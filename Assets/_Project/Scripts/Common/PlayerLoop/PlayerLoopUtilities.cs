using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace LD58
{
    public static class PlayerLoopUtilities
    {
        public static void InsertSubSystem<T>(ref PlayerLoopSystem system_to_insert, int index, bool register_for_clean_up = true)
        {
            InsertSubSystem(ref system_to_insert, typeof(T), index, register_for_clean_up);
        }

        public static void InsertSubSystem(ref PlayerLoopSystem system_to_insert, Type parent_system_type, int index, bool register_for_clean_up = true)
        {
            PlayerLoopSystem root_system = PlayerLoop.GetCurrentPlayerLoop();
            InsertSubSystemRecursive(ref root_system, ref system_to_insert, parent_system_type, index);
            PlayerLoop.SetPlayerLoop(root_system);

            if (register_for_clean_up)
            {
                PlayerLoopCleaner.RegisterForCleanUp(system_to_insert.type);
            }
        }

        private static bool InsertSubSystemRecursive(ref PlayerLoopSystem parent_system, ref PlayerLoopSystem system_to_insert, Type parent_system_type, int index)
        {
            if (parent_system.type == parent_system_type)
            {
                InsertSubSystem(ref parent_system, ref system_to_insert, index);

                return true;
            }

            if (parent_system.subSystemList == null)
            {
                return false;
            }

            for (int sub_system_index = 0; sub_system_index < parent_system.subSystemList.Length; sub_system_index++)
            {
                PlayerLoopSystem sub_system = parent_system.subSystemList[sub_system_index];

                if (InsertSubSystemRecursive(ref sub_system, ref system_to_insert, parent_system_type, index))
                {
                    parent_system.subSystemList[sub_system_index] = sub_system;

                    return true;
                }
            }

            return false;
        }

        private static void InsertSubSystem(ref PlayerLoopSystem parent_system, ref PlayerLoopSystem system_to_insert, int index)
        {
            List<PlayerLoopSystem> player_loop_systems = new();

            if (parent_system.subSystemList != null)
            {
                player_loop_systems.AddRange(parent_system.subSystemList);
            }

            index = Mathf.Clamp(index, 0, player_loop_systems.Count);
            player_loop_systems.Insert(index, system_to_insert);
            parent_system.subSystemList = player_loop_systems.ToArray();
        }

        public static void InsertSystemBefore<T>(ref PlayerLoopSystem system_to_insert, bool register_for_clean_up = true)
        {
            InsertSystemBefore(ref system_to_insert, typeof(T), register_for_clean_up);
        }

        public static void InsertSystemBefore(ref PlayerLoopSystem system_to_insert, Type insert_before_system, bool register_for_clean_up = true)
        {
            InsertSystemNextTo(ref system_to_insert, insert_before_system, insert_after: false, register_for_clean_up);
        }

        public static void InsertSystemAfter<T>(ref PlayerLoopSystem system_to_insert, bool register_for_clean_up = true)
        {
            InsertSystemAfter(ref system_to_insert, typeof(T), register_for_clean_up);
        }

        public static void InsertSystemAfter(ref PlayerLoopSystem system_to_insert, Type insert_after_system, bool register_for_clean_up = true)
        {
            InsertSystemNextTo(ref system_to_insert, insert_after_system, insert_after: true, register_for_clean_up);
        }

        private static void InsertSystemNextTo(ref PlayerLoopSystem system_to_insert, Type insert_next_to_system, bool insert_after, bool register_for_clean_up = true)
        {
            PlayerLoopSystem root_system = PlayerLoop.GetCurrentPlayerLoop();
            InsertSystemNextToRecursive(ref root_system, ref system_to_insert, insert_next_to_system, insert_after);
            PlayerLoop.SetPlayerLoop(root_system);

            if (register_for_clean_up)
            {
                PlayerLoopCleaner.RegisterForCleanUp(system_to_insert.type);
            }
        }

        private static bool InsertSystemNextToRecursive(ref PlayerLoopSystem parent_system, ref PlayerLoopSystem system_to_insert, Type insert_next_to_system, bool insert_after)
        {
            if (parent_system.subSystemList == null)
            {
                return false;
            }

            for (int sub_system_index = 0; sub_system_index < parent_system.subSystemList.Length; sub_system_index++)
            {
                PlayerLoopSystem sub_system = parent_system.subSystemList[sub_system_index];

                if (sub_system.type == insert_next_to_system)
                {
                    int insert_index = insert_after ? sub_system_index + 1 : sub_system_index;
                    InsertSubSystem(ref parent_system, ref system_to_insert, insert_index);

                    return true;
                }

                if (InsertSystemNextToRecursive(ref sub_system, ref system_to_insert, insert_next_to_system, insert_after))
                {
                    parent_system.subSystemList[sub_system_index] = sub_system;

                    return true;
                }
            }

            return false;
        }

        public static void RemoveSystem<T>()
        {
            RemoveSystem(typeof(T));
        }

        public static void RemoveSystem(Type system_to_remove)
        {
            PlayerLoopSystem root_system = PlayerLoop.GetCurrentPlayerLoop();
            RemoveSystemRecursive(ref root_system, system_to_remove);
            PlayerLoop.SetPlayerLoop(root_system);
        }

        private static bool RemoveSystemRecursive(ref PlayerLoopSystem parent_system, Type system_to_remove)
        {
            if (parent_system.subSystemList == null)
            {
                return false;
            }

            for (int sub_system_index = 0; sub_system_index < parent_system.subSystemList.Length; sub_system_index++)
            {
                PlayerLoopSystem sub_system = parent_system.subSystemList[sub_system_index];

                if (sub_system.type == system_to_remove)
                {
                    RemoveSubSystem(ref parent_system, sub_system_index);

                    return true;
                }

                if (RemoveSystemRecursive(ref sub_system, system_to_remove))
                {
                    parent_system.subSystemList[sub_system_index] = sub_system;

                    return true;
                }
            }

            return false;
        }

        public static void RemoveSubSystem(ref PlayerLoopSystem parent_system, int remove_index)
        {
            List<PlayerLoopSystem> player_loop_systems = new(parent_system.subSystemList);
            player_loop_systems.RemoveAt(remove_index);
            parent_system.subSystemList = player_loop_systems.ToArray();
        }

        public static void PrintPlayerLoop(string header)
        {
            StringBuilder string_builder = new();
            PlayerLoopSystem root_system = PlayerLoop.GetCurrentPlayerLoop();

            string_builder.AppendLine(header);
            PrintPlayerLoopRecursive(ref root_system, string_builder, 0);

            Debug.Log(string_builder.ToString());
        }

        private static void PrintPlayerLoopRecursive(ref PlayerLoopSystem parent_system, StringBuilder string_builder, int level)
        {
            if (parent_system.type != null)
            {
                string_builder.Append(' ', level * 2)
                    .AppendLine(parent_system.type.ToString());
            }

            if (parent_system.subSystemList != null)
            {
                for (int sub_system_index = 0; sub_system_index < parent_system.subSystemList.Length; sub_system_index++)
                {
                    PlayerLoopSystem sub_system = parent_system.subSystemList[sub_system_index];
                    PrintPlayerLoopRecursive(ref sub_system, string_builder, level + 1);
                }
            }
        }
    }
}
