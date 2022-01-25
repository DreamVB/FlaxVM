# Times table lister

jmp .Main

ProcSpace:
  PUSH 32
  SYS 4
  POP
RET

ProcCr:
 PUSH 10
 SYS 4
 POP
RET

TwoTimes:
 PUSH 2
 STA T
RET

.Main:
 CALL TwoTimes
 CALL PrintTable
 CALL ProcCr
HLT

PrintTable:
 PUSH 1
 STA A
 PUSH 12
 STA B

 loop:
  # Print A value to the console
  LDA A
  SYS 3
  CALL ProcSpace
  PUSH '*'
  SYS 4
  POP
  CALL ProcSpace
  LDA T
  SYS 3
  CALL ProcSpace
  PUSH '='
  SYS 4
  POP
  CALL ProcSpace
  LDA A
  LDA T
  MUL
  STA D
  LDA D
  SYS 3
  # Print break line
  CALL ProcCr
  LDA B
  LDA A
  INC
  STA A
  LDA A
  LEQ
  JT Loop
RET
