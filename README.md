# 6502 CPU Emulator

![6502 Emulator](https://github.com/maxs-rose/6502-CPU-Emulator/workflows/6502%20Emulator/badge.svg?branch=master)

This is my attempt at creating a 6502 CPU emulator in .NET Core

At some point I will also hopefully create a compiler for this as well.

## Resources
- [obelisk](http://www.obelisk.me.uk/6502/i)
- [C64 Wiki](https://www.c64-wiki.com/)

## Things Learned
- Emulating even a simple processor is very time consuming
- Understanding all of the addressing modes is hard


---

## Status of completion

- [ ] Instructions
  - Load Ops
  - [x] LDA
  - [x] LDX
  - [x] LDY
  - Store Ops
  - [x] STA
  - [x] STX
  - [x] STY
  - Transfer Ops
  - [x] TAX
  - [x] TAY
  - [x] TXA
  - [x] TYA
  - Stack Ops
  - [x] TSX
  - [x] TXS
  - [x] PHA
  - [x] PHP
  - [x] PLA
  - [x] PLP
  - Logical Ops
  - [ ] AND
  - [ ] EOR
  - [ ] ORA
  - [ ] BIT
  - Arithmetic Ops
  - [ ] ADC
  - [ ] SBC
  - [ ] CMP
  - [ ] CPX
  - [ ] CPY
  - Increment
  - [x] INC
  - [x] INX
  - [x] INY
  - Decrement
  - [ ] DEC
  - [ ] DEX
  - [ ] DEY
  - Shifts
  - [ ] ASL
  - [ ] LSR
  - [ ] ROL
  - [ ] ROR
  - Jumps & Calls
  - [x] JMP
  - [x] JSR
  - [x] RTS
  - Branches
  - [ ] BCC
  - [ ] BCS
  - [ ] BEQ
  - [ ] BMI
  - [ ] BNE
  - [ ] BPL
  - [ ] BVC
  - [ ] BVS
  - Status Flags Changes
  - [ ] CLC
  - [ ] CLD
  - [ ] CLI
  - [ ] CLV
  - [ ] SEC
  - [ ] SED
  - [ ] SEI
  - System Functions
  - [ ] BRK
  - [ ] NOP
  - [ ] RTI