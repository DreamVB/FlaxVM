# Find the max of two numbers

PUSH 10
STA A
PUSH 4
STA B

LDA A
LDA B
ISGT
JT Then

# Else
LDA A
STA C
JMP Exit

Then:
 LDA B
 STA C
 JMP Exit

Exit:
 LDA C
 SYS 3
 PUSH 10
 SYS 4
 POP
 HLT
