# Print all the printable ASCII characters

PUSH 32
STA A
PUSH 127
STA B

loop:
 # Print A value to the console
 LDA A
 SYS 4
 POP
 
 LDA B
 LDA A
 INC
 STA A
 LDA A
 LEQ
 JT Loop
 
PUSH 10
SYS 4
POP
HLT
