# TIME COLLISION


possible cases:
```
[A] exactly the same
    X |-------|            (01-09)
    Y |-------|            (01-09)

[B] ends faster 1
    X |-------|            (01-09)
    Y |--|                 (01-04)

[C] ends faster 2
    X |--|                 (01-04)
    Y |-------|            (01-09)

[D] starts later 1
    X |-------|            (01-09)
    Y      |--|            (06-09)

[E] starts later 2
    X      |--|            (06-09)
    Y |-------|            (01-09)

[F] in-between 1
    X |-------|            (01-09)
    Y   |--|               (03-06)

[G] in-between 2
    X   |--|               (03-06)
    Y |-------|            (01-09)

[H] have intersecting time 1
    X |-------|            (01-09)
    Y      |-------|       (06-15)

[I] have intersecting time 2
    X      |-------|       (06-15)
    Y |-------|            (01-09)

[J] touching boundary 1
    X |-------|            (01-09)
    Y         |-------|    (09-17)

[K] touching boundary 2
    X         |-------|    (09-17)
    Y |-------|            (01-09)

[L] earlier
    X           |-------|  (11-19)
    Y |-------|            (01-09)

[M] later
    X |-------|            (01-09)
    Y           |-------|  (11-19)

```

# EXAMPLE MYSQL TABLE TO TEST THE CASES
```
CREATE TABLE `TestTbl` (
  `name` varchar(5) NOT NULL,
  `t_start` time NOT NULL,
  `t_end` time NOT NULL,
  PRIMARY KEY (`name`)
) ENGINE=InnoDB;

INSERT INTO `TestTbl` (`name`, `t_start`, `t_end`) VALUES
  ('11-19', '11:00:00', '19:00:00'),
  ('01-04', '01:00:00', '04:00:00'),
  ('01-09', '01:00:00', '09:00:00'),
  ('03-06', '03:00:00', '06:00:00'),
  ('06-15', '06:00:00', '15:00:00'),
  ('06-09', '06:00:00', '09:00:00'),
  ('09-17', '09:00:00', '17:00:00')
;

CREATE TABLE `TestCase` (
  `casename` CHAR(1) NOT NULL ,
  `name_x` VARCHAR(5) NOT NULL ,
  `name_y` VARCHAR(5) NOT NULL ,
  PRIMARY KEY (`casename`)
) ENGINE = InnoDB;

INSERT INTO `TestCase` (`casename`, `name_x`, `name_y`) VALUES
  ('A', '01-09', '01-09'),
  ('B', '01-09', '01-04'),
  ('C', '01-04', '01-09'),
  ('D', '01-09', '06-09'),
  ('E', '06-09', '01-09'),
  ('F', '01-09', '03-06'),
  ('G', '03-06', '01-09'),
  ('H', '01-09', '06-15'),
  ('I', '06-15', '01-09'),
  ('J', '01-09', '09-17'),
  ('K', '09-17', '01-09'),
  ('L', '11-19', '01-09'),
  ('M', '01-09', '11-19')
;

```

# COMBINATION

#### not colliding (touching boundary allowed)
- true (4): `[J],[K],[L],[M]`
- false (9): `[A],[B],[C],[D],[E],[F],[G],[H],[I]`
- example use case: allocating time for resource (e.g.: room, CPU, etc)
```sql
SELECT *
FROM TestCase c
INNER JOIN TestTbl x ON c.name_x=x.name
INNER JOIN TestTbl y ON c.name_y=y.name
WHERE x.t_start >= y.t_end OR x.t_end <= y.t_start
```

#### not colliding (touching boundary not allowed)
- true (2): `[L],[M]`
- false (11): `[A],[B],[C],[D],[E],[F],[G],[H],[I],[J],[K]`
- example use case: allocating bucket/class for people according to ordinal attribute (e.g.: age 0-10 classified as child, 11-19 as teen, 20-999 as adult)
```sql
SELECT *
FROM TestCase c
INNER JOIN TestTbl x ON c.name_x=x.name
INNER JOIN TestTbl y ON c.name_y=y.name
WHERE x.t_start > y.t_end OR x.t_end < y.t_start
```

#### colliding (only touching boundary also included)
- true (11): `[A],[B],[C],[D],[E],[F],[G],[H],[I],[J],[K]`
- false (2): `[L],[M]`
- this is inverse of **not colliding (touching boundary not allowed)**
- example use case: disallow one people from being placed in multiple bucket/class according to ordinal attribute (negation of **not colliding (touching boundary not allowed)**)
```sql
SELECT *
FROM TestCase c
INNER JOIN TestTbl x ON c.name_x=x.name
INNER JOIN TestTbl y ON c.name_y=y.name
WHERE x.t_start <= y.t_end AND x.t_end >= y.t_start
```

#### colliding (only touching boundary not included)
- true (9): `[A],[B],[C],[D],[E],[F],[G],[H],[I]`
- false (4): `[J],[K],[L],[M]`
- this is inverse of **not colliding (touching boundary allowed)**
- example use case: disallow same resource usage in same allocated time (negation of **not colliding (touching boundary allowed)**)
```sql
SELECT *
FROM TestCase c
INNER JOIN TestTbl x ON c.name_x=x.name
INNER JOIN TestTbl y ON c.name_y=y.name
WHERE x.t_start < y.t_end AND x.t_end > y.t_start
```