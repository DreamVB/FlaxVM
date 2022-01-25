# Swap two items on the stack

PUSH 4
STA A
PUSH 5
STA B
#Print the original values
LDA A
SYS 3
PUSH 32
SYS 4
POP
LDA B
SYS 3
PUSH 10
SYS 4
POP
# Here we do the swap
LDA A
LDA B
SWAP
STA A
STA B

# Now print top swap items
LDA A
SYS 3
PUSH 32
SYS 4
POP
LDA B
SYS 3
PUSH 10
SYS 4
POP
HLT
