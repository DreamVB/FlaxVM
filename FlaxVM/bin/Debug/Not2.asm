# Bool not

PUSH 1
STA A
LDA A
PUSH 0
ISEQ
STA A
LDA A
SYS 3

HLT
