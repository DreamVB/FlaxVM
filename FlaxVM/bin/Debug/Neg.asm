# NEG
PUSH 5
NEG
SYS 3
call FuncCr
HLT

FuncCr:
 PUSH 10
 SYS 4
 POP
RET