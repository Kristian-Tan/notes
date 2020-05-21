# Bitwise Operator

## Definition

Bitwise operator compare two input as ```bit by bit comparison```

#### Notation:
- ```&``` is bitwise conjungtion operator
- ```|``` is bitwise disjunction operator
- ```~``` is bitwise negation operator
- ```^``` is bitwise exclusive or operator (xor)
- ```=``` is assignment operator
- ```0``` and ```1``` is the number 1 and 0 in binary
- ```r``` means that this bit is ```reserved``` for other purpose, or the value is unknown
    - do note that ```r & 0``` will always result in 0
    - do note that ```r | 1``` will always result in 1
    - do note that ```r ^ 1``` will always result in ```~r``` (example: ```1 xor 1 = 0```, ```0 xor 1 = 1```)
    - do note that ```r ^ 0``` will always result in ```r``` (example: ```1 xor 0 = 1```, ```0 xor 0 = 0```)

#### Example:
- ```7&5``` outputs ```5```, since 7 is ```111``` and 5 is ```101```, therefore the result is ```101```
- ```5&2``` outputs ```0```, since 5 is ```101``` and 2 is ```010```, therefore the result is ```000```


## Dealing with single digit memory

For example, a system have this memory block: ```mem = 00000000000000000000000000001000```

Your program may only use 1 bit of memory: the bit that is shown as ```1``` in the memory block (assume that other bit is allocated for other process)

- First, make a ```bit mask```: assign 0 to all unusable bit and 1 to usable bit
- Your bit mask: ```bitmask = 00000000000000000000000000001000 (binary) = 8 (decimal)```

#### Reading memory

- ```memCheck = mem & bitmask```
- Variable memCheck will contain ```00000000000000000000000000001000``` if the bit is 1, or contain ```00000000000000000000000000000000``` if the bit is 0
- Example: if the bit is 1:
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr1rrr``` (content of mem)
    - ```00000000000000000000000000001000``` (content of bitmask)
    - ```00000000000000000000000000001000``` (result of memCheck after AND operation)
- Example: if the bit is 0:
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr0rrr``` (content of mem)
    - ```00000000000000000000000000001000``` (content of bitmask)
    - ```00000000000000000000000000000000``` (result of memCheck after AND operation)

#### Set memory to 0

- When setting new value to the bit, other bit must not be changed (as they're used by other process)
- ```negBitmask = ~bitmask``` (negBitmask will contain ```11111111111111111111111111110111```)
- ```mem = mem & negBitmask```
- Example: if the bit is 1:
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr1rrr``` (content of mem)
    - ```11111111111111111111111111110111``` (content of negBitmask)
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr0rrr``` (result of mem after AND operation)
- Example: if the bit is 0:
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr0rrr``` (content of mem)
    - ```11111111111111111111111111110111``` (content of negBitmask)
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr0rrr``` (result of mem after AND operation)
- Explanation:
    - do note that ```r & 0``` will always result in 0

#### Set memory to 1

- When setting new value to the bit, other bit must not be changed (as they're used by other process)
- ```mem = mem | bitmask```
- Example: if the bit is 1:
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr1rrr``` (content of mem)
    - ```00000000000000000000000000001000``` (content of bitmask)
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr1rrr``` (result of mem after OR operation)
- Example: if the bit is 0:
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr0rrr``` (content of mem)
    - ```00000000000000000000000000001000``` (content of bitmask)
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr1rrr``` (result of mem after OR operation)
- Explanation:
    - do note that ```r | 1``` will always result in 1

#### Invert / negate the memory

- When setting new value to the bit, other bit must not be changed (as they're used by other process)
- ```mem = mem ^ bitmask```
- Example: if the bit is 1:
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr1rrr``` (content of mem)
    - ```00000000000000000000000000001000``` (content of bitmask)
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr0rrr``` (result of mem after XOR operation)
- Example: if the bit is 0:
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr0rrr``` (content of mem)
    - ```00000000000000000000000000001000``` (content of bitmask)
    - ```rrrrrrrrrrrrrrrrrrrrrrrrrrrr1rrr``` (result of mem after XOR operation)
- Explanation:
    - do note that ```r ^ 1``` will always result in ```~r``` (example: ```1 xor 1 = 0```, ```0 xor 1 = 1```)
    - do note that ```r ^ 0``` will always result in ```r``` (example: ```1 xor 0 = 1```, ```0 xor 0 = 0```)
