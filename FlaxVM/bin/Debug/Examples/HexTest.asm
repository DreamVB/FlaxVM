# Using hexidecimal as values 50 + 50
# 50 in hex is 0x32

PUSH 0x32
PUSH 0x32
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