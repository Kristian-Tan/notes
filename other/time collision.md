# TIME COLLISION

#### exactly the same
```
A |---------|
B |---------|
```
identify with: A.start = B.start AND A.end = B.end

#### ends faster
```
A |---------|
B |----|
```
identify with: A.start = B.start AND A.end > B.end

#### starts later
```
A |---------|
B      |----|
```
identify with: A.start < B.start AND A.end = B.end

#### in-between
```
A |---------|
B   |----|
```
identify with: A.start < B.start AND A.end > B.end

#### have intersecting time
```
A |---------|
B      |---------|
```
identify with: A.start < B.start AND A.end < B.end AND A.end > B.start



#### (revision) have intersecting time
```
A |---------|
B      |---------|
```
or
```
A      |---------|
B |---------|
```
B.start BETWEEN A.start AND A.end (1)
OR
A.start BETWEEN B.start AND B.end (2)
OR
A.end BETWEEN B.start AND B.end (1)
OR
B.end BETWEEN A.start AND A.end (2)



#### ends faster (invert)
```
A |----|
B |---------|
```
identify with: A.start = B.start AND A.end < B.end

#### starts later (invert)
```
A      |----|
B |---------|
```
identify with: A.start > B.start AND A.end = B.end

#### in-between (invert)
```
A   |----|
B |---------|
```
identify with: A.start > B.start AND A.end < B.end

#### have intersecting time (invert)
```
A      |---------|
B |---------|
```
identify with: A.start > B.start AND A.end > B.end AND A.start < B.end

# TIME BOUNDARY

#### touching boundary
```
A |---------|
B           |---------|
```
identify with: A.end = B.start

#### touching boundary (invert)
```
A           |---------|
B |---------|
```
identify with: A.start = B.end

# TIME NON-COLLISION

#### earlier
```
A                |---------|
B |---------|
```
identify with: A.start > B.end

#### later
```
A |---------|
B                |---------|
```
identify with: A.end < B.start

# COMBINATION

#### not colliding
- earlier OR later
- A.start > B.end OR A.end < B.start

#### colliding
- NOT (earlier OR later)
- NOT earlier AND NOT later
- A.start <= B.end AND A.end >= B.start

#### colliding non-inclusive
- NOT (earlier OR later) AND NOT (touching boundary)
- NOT earlier AND NOT later AND NOT (touching boundary)
- A.start <= B.end AND A.end >= B.start AND NOT (touching boundary)
- A.start <= B.end AND A.end >= B.start AND NOT (A.end = B.start OR A.start = B.end)
- A.start <= B.end AND A.end >= B.start AND NOT A.end = B.start AND NOT A.start = B.end
- A.start <= B.end AND A.end >= B.start AND A.end != B.start AND A.start != B.end
- A.start <= B.end AND A.end > B.start AND A.start != B.end
- A.start < B.end AND A.end > B.start
