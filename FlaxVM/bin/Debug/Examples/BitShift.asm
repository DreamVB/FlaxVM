# SHL SHR

#Bitshift left
PUSH 4
PUSH 2
SHL
SYS 3
call FuncCr

#Shift right
PUSH 4
PUSH 128
SHR
SYS 3
CALL FuncCr
HLT

FuncCr:
  PUSH 10
  SYS 4
  POP
  RET
