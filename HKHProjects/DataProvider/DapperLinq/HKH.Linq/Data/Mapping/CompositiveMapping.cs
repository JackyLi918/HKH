using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HKH.Linq.Data.Common;

namespace HKH.Linq.Data.Mapping
{
    public class CompositiveMapping : ImplicitMapping
    {
        Dictionary<string, MappingEntity> entities = new Dictionary<string, MappingEntity>();
        ReaderWriterLock rwLock = new ReaderWriterLock();

        public CompositiveMapping()
        {
        }

        public override MappingEntity GetEntity(Type elementType, string tableId)
        {
            if (tableId == null)
                tableId = GetTableId(elementType);

            MappingEntity entity;
            rwLock.AcquireReaderLock(Timeout.Infinite);
            if (!entities.TryGetValue(tableId, out entity))
            {
                rwLock.ReleaseReaderLock();
                rwLock.AcquireWriterLock(Timeout.Infinite);
                if (!entities.TryGetValue(tableId, out entity))
                {
                    entity = new CompositiveMappingEntity(elementType, tableId);
                    this.entities.Add(tableId, entity);
                }
                rwLock.ReleaseWriterLock();
            }
            else
            {
                rwLock.ReleaseReaderLock();
            }

            return entity;
        }

        public override string GetTableId(Type type)
        {
            return type.Name;
        }

        public override bool IsPrimaryKey(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.IsPrimaryKey;

            return base.IsPrimaryKey(entity, member);
        }

        public override bool IsColumn(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null)
                return !mm.NotMapped;

            return base.IsColumn(entity, member);
        }
        public override bool IsComputed(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.IsComputed;

            return base.IsComputed(entity, member);
        }

        public override bool IsGenerated(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.IsGenerated;

            return base.IsGenerated(entity, member);
        }
        public override bool IsMapped(MappingEntity entity, MemberInfo member)
        {
            return IsColumn(entity, member);
        }

        public override string GetTableName(MappingEntity entity)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            if (en.Table != null)
                return en.Table.Name;

            return base.GetTableName(entity);
        }

        public override string GetColumnName(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.Name;

            return base.GetColumnName(entity, member);
        }

        class CompositiveMappingEntity : MappingEntity
        {
            string entityID;
            Type type;
            Dictionary<string, CompositiveMappingMember> mappingMembers;

            public CompositiveMappingEntity(Type type, string entityID)
            {
                this.entityID = entityID;
                this.type = type;

                CollectMappingMembers();
            }

            public override string TableId
            {
                get { return this.entityID; }
            }

            public override Type ElementType
            {
                get { return this.type; }
            }

            public override Type EntityType
            {
                get { return this.type; }
            }

            internal TableAttribute Table { get; private set; }

            internal CompositiveMappingMember GetMappingMember(string name)
            {
                CompositiveMappingMember mm = null;
                this.mappingMembers.TryGetValue(name, out mm);
                return mm;
            }

            private void CollectMappingMembers()
            {
                Table = Attribute.GetCustomAttribute(EntityType, typeof(TableAttribute)) as TableAttribute;

                mappingMembers = new Dictionary<string, CompositiveMappingMember>();
                foreach (MemberInfo m in EntityType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    CreateMappingMember(m);
                }
                foreach (MemberInfo m in EntityType.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    CreateMappingMember(m);
                }
            }

            private void CreateMappingMember(MemberInfo m)
            {
                if (m.CustomAttributes.Count() > 0)
                {
                    foreach (MemberAttribute attr in Attribute.GetCustomAttributes(m, typeof(MemberAttribute)) as MemberAttribute[])
                    {
                        if (string.IsNullOrEmpty(attr.Member))
                            attr.Member=m.Name;

                        if (mappingMembers.ContainsKey(m.Name))
                            throw new InvalidOperationException(string.Format("AttributeMapping: more than one mapping attribute specified for member '{0}' on type '{1}'", m.Name, EntityType.Name));

                        if (attr is ColumnAttribute)
                        {
                            var colAttr = attr as ColumnAttribute;
                            if (string.IsNullOrEmpty(colAttr.Name))
                                colAttr.Name = m.Name;
                        }

                        mappingMembers.Add(m.Name, new CompositiveMappingMember(m, attr));
                    }
                }
            }
        }

        internal class CompositiveMappingMember
        {
            MemberInfo member;
            MemberAttribute attribute;

            internal CompositiveMappingMember(MemberInfo member, MemberAttribute attribute)
            {
                this.member = member;
                this.attribute = attribute;
            }

            internal MemberInfo Member
            {
                get { return this.member; }
            }

            internal bool NotMapped
            {
                get { return this.attribute is NotMappedAttribute; }
            }

            internal ColumnAttribute Column
            {
                get { return this.attribute as ColumnAttribute; }
            }

            internal AssociationAttribute Association
            {
                get { return this.attribute as AssociationAttribute; }
            }
        }
    }
}