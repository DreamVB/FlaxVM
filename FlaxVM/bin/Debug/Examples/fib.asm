# Fibonacci Series numbers 1 to 13

# I = 0
PUSH 0
STA I

# X = 13
PUSH 13
STA X

#A = 0
PUSH 0
STA A

#B = 1
PUSH 1
STA B

# C = 0
PUSH 0
STA C

While:
 # C = A + B
 LDA A
 LDA B
 ADD
 STA C
 #Display fibonacci numbers
 LDA C
 SYS 3
 CALL ProcSP
 # A = B
 LDA B
 STA A
 # B = C
 LDA C
 STA B
 # I++
 LDA X
 LDA I
 INC
 #Store at I
 STA I
 LDA I
 #Is I < X
 ISLT
 #If true jump to while label
 JT While
HLT

ProcSP:
 PUSH 32
 SYS 4
 POP
RET
