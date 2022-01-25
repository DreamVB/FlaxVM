# Times table lister that lists tables 1 to 12

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

.Main:
 PUSH 1
 STA X
 PUSH 12
 STA Y

 While:
  LDA X
  STA T
  CALL PrintTable
  CALL ProcCr
  LDA Y
  LDA X
  INC
  STA X
  LDA X
  LEQ
  JT While
HLT