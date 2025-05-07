using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
//using static SO_Dumper.SObj;
using static SO_Dumper.Util;

namespace SO_Dumper
{
    internal class CSimpleObjects
    {
        /*
         * Chrome Engine 5
        public uint m_NumEntitiesSum;
        public uint m_NumPreRenderBatches;
        public uint m_NumBatchTreeNodes;
        public uint m_NumTypes;
        public uint m_NumMeshes;
        public uint m_MeshDataSize;

        public void Deserialize(Stream input)
        {
            m_NumEntitiesSum = Util.ReadValueU32(input);
            m_NumPreRenderBatches = Util.ReadValueU32(input);
            m_NumBatchTreeNodes = Util.ReadValueU32(input);
            m_NumTypes = Util.ReadValueU32(input);
            m_NumMeshes = Util.ReadValueU32(input);
            m_MeshDataSize = Util.ReadValueU32(input);
        }
        public void Serialize(Stream output)
        {
            Util.WriteU32(output, m_NumEntitiesSum);
            Util.WriteU32(output, m_NumPreRenderBatches);
            Util.WriteU32(output, m_NumBatchTreeNodes);
            Util.WriteU32(output, m_NumTypes);
            Util.WriteU32(output, m_NumMeshes);
            Util.WriteU32(output, m_MeshDataSize);
        }
        */
        public uint m_NumEntitiesSum;
        public uint m_NumTypes;
        public uint m_NumMeshes;
        public uint m_unk_1;
        public uint m_unk_2;
        public uint m_unk_3;
        public uint m_unk_4;
        public uint m_unk_5;
        public float m_MaxVisibilityRange; 
        
        public extents m_Extents;

        public AABB m_AABB;

        public SType_Render[] m_TypesRender;
        public string MeshElement;

        public SEntity[] m_Entities;
        public void Deserialize(Stream input)
        {
            m_NumEntitiesSum = Util.ReadValueU32(input);
            m_NumTypes = Util.ReadValueU32(input);
            m_NumMeshes = Util.ReadValueU32(input);
            m_unk_1 = Util.ReadValueU32(input);
            m_unk_2 = Util.ReadValueU32(input);
            m_unk_3 = Util.ReadValueU32(input);
            m_unk_4 = Util.ReadValueU32(input);
            m_unk_5 = Util.ReadValueU32(input);
            m_MaxVisibilityRange = Util.ReadFloat(input);

            m_Extents = new extents();
            m_Extents.Deserialize(input);


            m_AABB = new AABB();
            //m_AABB.Deserialize(input);


            m_AABB.origin = new vec3
            {
                x = (m_Extents.min.x + m_Extents.max.x) * 0.5f,
                y = (m_Extents.min.y + m_Extents.max.y) * 0.5f,
                z = 0.5f * (m_Extents.min.z + m_Extents.max.z)
            };

            m_AABB.span = new vec3
            {
                x = m_Extents.max.x - m_AABB.origin.x,
                y = m_Extents.max.y - m_AABB.origin.y,
                z = m_Extents.max.z - m_AABB.origin.z,
            };

            if (m_NumMeshes != 0)
            {
                m_TypesRender = new SType_Render[m_NumMeshes];

                for (uint i = 0; i < m_NumMeshes; i++)
                {
                    m_TypesRender[i] = new SType_Render();
                    m_TypesRender[i].Deserialize(input);


                    //discard the following
                    //section end, int, -1
                    _ = Util.ReadValueU32(input);
                    //Mesh Element, stringChrome
                    MeshElement = Util.ReadStringChrome(input, Encoding.ASCII);
                    //Short Could be somehow related to mesh_spu, pretty sure always 00
                    _ = Util.ReadValueU16(input);
                }
            }
            if (m_NumEntitiesSum != 0)
            {
                m_Entities = new SEntity[m_NumEntitiesSum];
                for (uint i = 0; i < m_NumEntitiesSum; i++)
                {
                    m_Entities[i] = new SEntity();
                    m_Entities[i].Deserialize(input);

                    //Discard pading pretty sure
                    _ = Util.ReadValueU32(input);
                }
            }
        }
        public void Serialize(Stream output)
        {
            Util.WriteU32(output, m_NumEntitiesSum);
            Util.WriteU32(output, m_NumTypes);
            Util.WriteU32(output, m_NumMeshes);
            Util.WriteU32(output, m_unk_1);
            Util.WriteU32(output, m_unk_2);
            Util.WriteU32(output, m_unk_3);
            Util.WriteU32(output, m_unk_4);
            Util.WriteU32(output, m_unk_5);
            Util.WriteFloat(output, m_MaxVisibilityRange);

            m_Extents.Serialize(output);

            for (uint i = 0; i < m_NumMeshes; i++)
            {
                m_TypesRender[i].Serialize(output);

                Util.WriteS32(output, -1);
                Util.WriteStringChrome(output, MeshElement, Encoding.ASCII);
                Util.WriteU16(output, 0);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("=== CSimpleObjects Data ===");
            sb.AppendLine($"NumEntitiesSum: {m_NumEntitiesSum}");
            sb.AppendLine($"NumTypes: {m_NumTypes}");
            sb.AppendLine($"NumMeshes: {m_NumMeshes}");
            sb.AppendLine($"Unknown 1: {m_unk_1}");
            sb.AppendLine($"Unknown 2: {m_unk_2}");
            sb.AppendLine($"Unknown 3: {m_unk_3}");
            sb.AppendLine($"Unknown 4: {m_unk_4}");
            sb.AppendLine($"Unknown 5: {m_unk_5}");
            sb.AppendLine();

            sb.AppendLine($"MaxVisibilityRange: {m_MaxVisibilityRange}");
            sb.AppendLine($"Extents: {m_Extents}");
            sb.AppendLine($"AABB: {m_AABB}");
            sb.AppendLine();


            if (m_TypesRender == null)
            {
                sb.AppendLine("No Types Render data available.");
            }
            for (int i = 0; i < m_TypesRender.Length; i++)
            {
                /*
                sb.AppendLine($"Render Type {i}:");
                sb.AppendLine($"  Mesh: {m_TypesRender[i].Mesh}");
                sb.AppendLine($"  Skin: {m_TypesRender[i].Skin}");
                sb.AppendLine($"  Required Tags: {m_TypesRender[i].required_tags}");
                sb.AppendLine($"  Forbidden Tags: {m_TypesRender[i].forbidden_tags}");
                sb.AppendLine($"  Seed: {m_TypesRender[i].seed}");
                sb.AppendLine($"  Collision Collide Bits: {m_TypesRender[i].collision_collide_bits}");
                sb.AppendLine();
                */
                sb.AppendLine($"Render Type {i}:");
                sb.AppendLine($"{m_TypesRender[i]}");
            }

            if (m_Entities == null)
            {
                sb.AppendLine("No Types Render data available.");
            }
            for (int i = 0; i < m_Entities.Length; i++)
            {
                sb.AppendLine($"Entity {i}:");
                sb.AppendLine($"{m_Entities[i]}");
            }


            return sb.ToString();
        }
    }
}
