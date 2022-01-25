# Using Variables names with more than one letter

#Declare variables here
# [n] i the variable index in memory
# text is the name of the variable
.Global ([0] Num1, [1] Num2, [2] Result)

PUSH 200
STA Num1
PUSH 50
STA Num2
LDA Num1
LDA Num2
ADD
DUP
ADD
STA Result
LDA Result
SYS 3
PUSH '\n'
SYS 4
POP

HLT
