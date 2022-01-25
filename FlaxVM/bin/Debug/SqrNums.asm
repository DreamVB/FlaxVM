# Square numbers 1 to 20
PUSH 1
STA A

#Get the user to enter a number of numbers to square
SYS 2
STA B

loop:
 # Print A value to the console
 LDA A
 DUP
 MUL
 SYS 3
 # Print break line
 CALL FuncCr
 
 LDA B
 LDA A
 INC
 STA A
 LDA A
 LEQ
 JT Loop
HLT

FuncCr:
  PUSH 10
  SYS 4
  POP
  RET