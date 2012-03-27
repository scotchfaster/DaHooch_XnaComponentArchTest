using System;

[Flags]
public enum CoordinateType
{
	None 	= 0x00,
	World	= 0x01,
	Screen	= 0x02,
	Both	= 0xFF
}