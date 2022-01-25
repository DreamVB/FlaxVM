# Add to a number using SUB
PUSH 10
SYS 3
CALL FuncCr

PUSH 10
PUSH -1
SUB
SYS 3

CALL FuncCr
HLT

FuncCr:
 PUSH 10
 SYS 4
 POP
RET