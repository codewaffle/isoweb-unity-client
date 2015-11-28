﻿namespace isoweb
{
    public enum PacketType : byte {
        NULL = 0,
        PING = 1,
        PONG = 2,
        MESSAGE = 3,
        META = 4,

        CMD_CONTEXTUAL_ENTITY = 10,
        CMD_CONTEXTUAL_POSITION = 11,
        CMD_MENU_REQ_ENTITY = 12,
        CMD_MENU_EXEC_ENTITY = 13,
        CMD_MENU_REQ_POSITION = 14,
        CMD_MENU_EXEC_POSITION = 15,

        DO_ASSIGN_CONTROL = 51,

        ENTITYDEF_UPDATE = 80,

        ENTITY_UPDATE = 100,
        POSITION_UPDATE = 101,
        STRING_UPDATE = 102,
        FLOAT_UPDATE = 103,
        INT_UPDATE = 104,
        BYTE_UPDATE = 105,
        HALF_UPDATE = 106,
        ENTITYDEF_HASH_UPDATE = 107,
        PARENT_UPDATE = 108,

        ENTITY_ENABLE = 120,
        ENTITY_DISABLE = 121,
        ENTITY_DESTROY = 122,

        CONTAINER_UPDATE = 130,
        CONTAINER_SHOW = 131,
        CONTAINER_HIDE = 132,

        CRAFT_LIST = 140,
        CRAFT_VIEW = 141,
        CRAFT_EXEC = 142

    }
}