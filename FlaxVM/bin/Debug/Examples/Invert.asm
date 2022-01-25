#Invert a value on the stack

PUSH 2
STA A
LDA A
NOT
STA B
LDA B
SYS 3
HLT
