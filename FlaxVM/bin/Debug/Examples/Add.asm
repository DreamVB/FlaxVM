# Count to 24 incrementing M by 2
PUSH 0
STA A
PUSH 12
STA B
PUSH 2
STA M

loop:

 LDA M
 SYS 3
 PUSH 10
 SYS 4
 POP

 # M*=2
 LDA M
 PUSH 2
 ADD
 STA M
 
 LDA B
 LDA A
 INC
 STA A
 LDA A
 ISLT
 JT Loop
HLT
