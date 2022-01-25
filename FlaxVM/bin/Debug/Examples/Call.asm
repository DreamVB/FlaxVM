# Use Call to add two numbers

JMP .Main

.Main:
  PUSH 10
  PUSH 30
  CALL FuncAdd
  CALL FuncCr
  HLT

FuncAdd:
 ADD
 SYS 3
 RET

FuncCr:
 PUSH 10
 SYS 4
 POP
 RET