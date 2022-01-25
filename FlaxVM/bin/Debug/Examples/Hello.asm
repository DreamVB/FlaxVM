JMP .Main

FuncCr:
 PUSH 10
 SYS 4
 POP
 RET

.Main:
 PUSH 0
 PUSH '!'
 PUSH 'd'
 PUSH 'l'
 PUSH 'r'
 PUSH 'o'
 PUSH 'w'
 PUSH 32
 PUSH 'o'
 PUSH 'l'
 DUP
 PUSH 'e'
 PUSH 'H'

ProcPrint:
 SYS 4
 POP
 JNZ ProcPrint
 POP
 CALL FuncCr
HLT



