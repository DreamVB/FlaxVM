# Use REM to find remainder

PUSH 3
STA A
PUSH 2
STA B
PUSH 0
STA C

LDA A
LDA B
REM
STA C

# Print out value
LDA C
SYS 3

HLT
