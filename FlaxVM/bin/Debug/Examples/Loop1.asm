# Print 1 to 10
PUSH 1
STA A
PUSH 10
STA B

loop:
 # Print A value to the console
 LDA A
 SYS 3
 # Print break line
 PUSH 10
 SYS 4
 POP
 
 LDA B
 LDA A
 INC
 STA A
 LDA A
 LEQ
 JT Loop

HLT
