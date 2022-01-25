# Print 10 down to 1
PUSH 10
STA A
PUSH 1
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
 DEC
 STA A
 LDA A
 GEQ
 JT Loop

HLT
