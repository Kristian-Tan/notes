# PostgreSQL Full-Text Search
source:
- http://rachbelaid.com/postgres-full-text-search-is-good-enough/
- https://www.compose.com/articles/mastering-postgresql-tools-full-text-search-and-phrase-search/
- https://blog.lateral.io/2015/05/full-text-search-in-milliseconds-with-postgresql/

## Feature:
- stemming? YES (multiple language support, may add new language from 3rd party repository)
- tokenizing? YES
- matching with score? NO
- sort based on similarity? YES
- must vectorize before query? NO (but it's recommended)

## Example
```sql
-- alternatif 1
CREATE TABLE post(
	id SERIAL PRIMARY KEY,
	title TEXT NOT NULL,
	content TEXT NOT NULL
);

INSERT INTO post (id, title, content)
VALUES (1, 'Endangered species',
		'Pandas are an endangered species'),
		(2, 'Freedom of Speech',
		'Freedom of speech is a necessary right'),
		(3, 'Star Wars vs Star Trek',
		'Few words from a big fan')
;

SELECT post.id, post.title, post.content, to_tsvector(post.content)
FROM post
WHERE to_tsvector(post.content) @@ to_tsquery('Endangered & Species')
ORDER BY ts_rank(to_tsvector(post.content), to_tsquery('Endangered & Species')) DESC;


-- alternatif 2
CREATE TABLE post_with_tsvector(
	id SERIAL PRIMARY KEY,
	title TEXT NOT NULL,
	content TEXT NOT NULL,
	tsvector_content TSVECTOR
);

INSERT INTO post_with_tsvector (id, title, content)
VALUES (1, 'Endangered species',
		'Pandas are an endangered species'),
		(2, 'Freedom of Speech',
		'Freedom of speech is a necessary right'),
		(3, 'Star Wars vs Star Trek',
		'Few words from a big fan')
;
UPDATE post_with_tsvector SET tsvector_content=to_tsvector(content) WHERE tsvector_content IS NULL;

SELECT id, title, content, tsvector_content
FROM post_with_tsvector
WHERE tsvector_content @@ to_tsquery('Endangered & Species')
ORDER BY ts_rank(tsvector_content, to_tsquery('Endangered & Species')) DESC;


/*
# tanpa stemming:
https://stackoverflow.com/questions/42052173/remove-stop-words-without-stemming-in-postgresql

## setting 1x awal:
CREATE TEXT SEARCH DICTIONARY simple_english
   (TEMPLATE = pg_catalog.simple, STOPWORDS = english);

CREATE TEXT SEARCH CONFIGURATION simple_english
   (copy = english);
ALTER TEXT SEARCH CONFIGURATION simple_english
   ALTER MAPPING FOR asciihword, asciiword, hword, hword_asciipart, hword_part, word
   WITH simple_english;

## saat meng-query:
SELECT to_tsvector('simple_english', 'many an ox eats the houses');
-- result: 'eats':4 'houses':5 'many':1 'ox':3
*/
```

## Simple Mode
use ```to_tsvector('simple', content)``` to ignore stemming and stopwords removal (useful when not using postgres supported language)

## Wildcard
- use ```:*``` after keyword to define wildcard suffix
- use ```|``` or ```&``` to separate words, and ```(``` and ```)``` to group operators
- use ```!``` to exclude word

## Hack to use prefix wildcard
- use this when the target language is not English (e.g.: Indonesian, where stemming doesn't work and words are prefixed and suffixed)
- general idea: run a normal fulltext search (e.g.: "keyword:\*") and a reversed fulltext search (e.g.: "drowyek:\*")
    - the inputted keyword must also be reversed when searching the reversed tsvector (hint: php function strrev)
- also save a reversed tsvector column for faster search
- use 'simple' mode rather than 'english' mode for making tsvector
```sql
CREATE TABLE post_with_tsvector(
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    content TEXT NOT NULL,
    tsvector_content TSVECTOR,
    tsvector_content_reversed TSVECTOR
);

INSERT INTO post_with_tsvector (id, title, content)
VALUES (1, 'Endangered species',
        'Pandas are an endangered species'),
        (2, 'Freedom of Speech',
        'Freedom of speech is a necessary right'),
        (3, 'Star Wars vs Star Trek',
        'Few words from a big fan')
;
UPDATE post_with_tsvector SET tsvector_content=to_tsvector('simple', content) WHERE tsvector_content IS NULL;
UPDATE post_with_tsvector SET tsvector_content_reversed=to_tsvector('simple', reverse(content)) WHERE tsvector_content_reversed IS NULL;

SELECT
    id, title, content,
    tsvector_content, tsvector_content_reversed,
    ts_rank(tsvector_content, to_tsquery('Endan:* | Spec:*')) AS similarity,
    ts_rank(tsvector_content_reversed, to_tsquery('nadnE:* | cepS:*')) AS similarity_reversed
FROM post_with_tsvector
WHERE
    tsvector_content @@ to_tsquery('Endan:* | Spec:*')
    OR tsvector_content_reversed @@ to_tsquery('nadnE:* | cepS:*')
ORDER BY
    (
        ts_rank(tsvector_content, to_tsquery('Endan:* | Spec:*')) +
        ts_rank(tsvector_content_reversed, to_tsquery('nadnE:* | cepS:*'))
    ) DESC;

```
