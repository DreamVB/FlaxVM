# Using binary as values 20 + 40

PUSH 00010100b
PUSH 00101000b
Add
#System call using a hex number
Sys 0x3
Call FuncCr
Hlt

FuncCr:
 PUSH 0xA
 SYS 4
 POP
RET