# MySQL Full-Text Search
source:
- https://www.digitalocean.com/community/tutorials/how-to-improve-database-searches-with-full-text-search-in-mysql-5-6-on-ubuntu-16-04
- https://www.mullie.eu/mysql-as-a-search-engine/
- https://mariadb.com/kb/en/full-text-index-overview/

## Feature:
- stemming? NO
- tokenizing? YES
- matching with score? YES
- sort based on similarity? YES
- must set index before query? YES

## Example
preparation:
```sql
-- create table
CREATE TABLE news (
   id INT NOT NULL AUTO_INCREMENT,
   title TEXT NOT NULL,
   content TEXT NOT NULL,
   author TEXT NOT NULL,
   PRIMARY KEY (id)
);

-- insert data
INSERT INTO news (id, title, content, author) VALUES
    (1, 'Pacific Northwest high-speed rail line', 'Currently there are only a few options for traveling the 140 miles between Seattle and Vancouver and none of them are ideal.', 'Greg'),
    (2, 'Hitting the beach was voted the best part of life in the region', 'Exploring tracks and trails was second most popular, followed by visiting the shops and then traveling to local parks.', 'Ethan'),
    (3, 'Machine Learning from scratch', 'Bare bones implementations of some of the foundational models and algorithms.', 'Jo');

-- define a fulltext index (need to be done once)
ALTER TABLE news ADD FULLTEXT (title, content, author);
```

query:
1. plain select *
```sql
select * from news \G
```
```
*************************** 1. row ***************************
     id: 1
  title: Pacific Northwest high-speed rail line
content: Currently there are only a few options for traveling the 140 miles between Seattle and Vancouver and none of them are ideal.
 author: Greg
*************************** 2. row ***************************
     id: 2
  title: Hitting the beach was voted the best part of life in the region
content: Exploring tracks and trails was second most popular, followed by visiting the shops and then traveling to local parks.
 author: Ethan
*************************** 3. row ***************************
     id: 3
  title: Machine Learning from scratch
content: Bare bones implementations of some of the foundational models and algorithms.
 author: Jo
3 rows in set (0.03 sec)
```

##### Conclusion: ```\\G``` outputs query in form-like format rather than table-like format

2. select with similarity score
```sql
SELECT *, MATCH (title,content,author) AGAINST ('Seattle beach visiting' IN NATURAL LANGUAGE MODE) AS match1 FROM news WHERE MATCH (title,content,author) AGAINST ('Seattle beach visiting' IN NATURAL LANGUAGE MODE) \G
```
```
*************************** 1. row ***************************
     id: 2
  title: Hitting the beach was voted the best part of life in the region
content: Exploring tracks and trails was second most popular, followed by visiting the shops and then traveling to local parks.
 author: Ethan
 match1: 0.45528939366340637
*************************** 2. row ***************************
     id: 1
  title: Pacific Northwest high-speed rail line
content: Currently there are only a few options for traveling the 140 miles between Seattle and Vancouver and none of them are ideal.
 author: Greg
 match1: 0.22764469683170319
2 rows in set (0.05 sec)
```

##### Conclusion: record id=2 contains 3 of 3 words in the query, while record id=1 contains 2 of 3 words in the query. Record id=3 is not shown because it have no words in the query. Also note that record id=2 rank higher than record id=1 because of that.

3. select with similarity score for words with same stem word
```sql
SELECT *, MATCH (title,content,author) AGAINST ('Seattle beach visit' IN NATURAL LANGUAGE MODE) AS match1 FROM news WHERE MATCH (title,content,author) AGAINST ('Seattle beach visit' IN NATURAL LANGUAGE MODE) \G
```
```
*************************** 1. row ***************************
     id: 1
  title: Pacific Northwest high-speed rail line
content: Currently there are only a few options for traveling the 140 miles between Seattle and Vancouver and none of them are ideal.
 author: Greg
 match1: 0.22764469683170319
*************************** 2. row ***************************
     id: 2
  title: Hitting the beach was voted the best part of life in the region
content: Exploring tracks and trails was second most popular, followed by visiting the shops and then traveling to local parks.
 author: Ethan
 match1: 0.22764469683170319
2 rows in set (0.00 sec)
```

##### Conclusion: mysql full-text search dnesn't stem words since ```visit``` and ```visiting``` keyword yields different result

## BOOLEAN MODE
```IN NATURAL LANGUAGE MODE``` keyword may be changed into ```IN BOOLEAN MODE``` in order to use boolean mode. Operators:

- ```+``` Word MUST be present in the text
- ```-``` Word MUST NOT be present in the text
- ```[nothing]``` Optionally includes this word
- ```~``` Decrase relevance if word exist
- ```@[distance]``` Search terms should appear within distance words of each other. E.g.: ```MATCH(col1) AGAINST('"word1 word2 word3" @8' IN BOOLEAN MODE)``` means that word1, word2 and word3 should all appear within 8-words range
- ```>``` or ```<``` Increases or decreases a word's importance in the relevance score
- ```(``` and ```)``` Groups words into a subexpression/phrase
- ```*``` Wildcard character, matches anything that begins with the word. E.g.: ```ho*```
- ```"``` Everything inside double quotes must be matched exactly
