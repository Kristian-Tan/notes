# PHP: use ADODB Database Abstraction Layer Library

## STEP 1: INSTALL ADODB:
download from http://sourceforge.net/projects/adodb/files and put it in webserver

## STEP 2: INCLUDING IN PROJECT
```php
<?php
define("ADODBPATH", "path_to_adodb_library"); //change this constants to make it point to ADODB installation path
require_once ADODBPATH . '/adodb.inc.php';
    //example:
    define("ADODBPATH", $_SERVER["DOCUMENT_ROOT"] . "/lib/adodb5");
    require_once ADODBPATH . '/adodb.inc.php';
```

## STEP 3: CONNCECT

- for MySQL choose one of 
  - 'mysqli', 
  - 'pdo_mysql'
  - (avoid to use old MySQL drivers 'mysqlt, 'mysql')
- for PostGreSQL use 'postgres'
- for Firebird use 'firebird'

```php
<?php
$DBType = 'mysqli';
$DBServer = 'mysql_server'; // server name or IP address
$DBUser = 'mysql_username'; //mysql username
$DBPass = rawurlencode('mysql_password'); //mysql password
$DBName = 'mysql_database_name'; //mysql database name
$dsn_options='?persist=0&fetchmode=2'; // 1=ADODB_FETCH_NUM, 2=ADODB_FETCH_ASSOC, 3=ADODB_FETCH_BOTH; (RECOMMENDED 2 (fetch assoc))
$dsn = "$DBType://$DBUser:$DBPass@$DBServer/$DBName$dsn_options";
$conn = NewADOConnection($dsn);
//TIPS: just put step 2 and step 3 in config.php in your webapp and make every other page require//require_onceinclude the config.php file

    //example:
    $DBType = 'mysqli';
    $DBServer = 'localhost';
    $DBUser = 'root';
    $DBPass = rawurlencode('my password');
    $DBName = 'db_my_database';
    $dsn_options='?persist=0&fetchmode=2';
    $dsn = "$DBType://$DBUser:$DBPass@$DBServer/$DBName$dsn_options";
    $conn = NewADOConnection($dsn);
```

## HOW TO USE: (S) SELECT
```php
<?php
$sql = 'SELECT col1_name, col1_name, col1_name FROM table1 WHERE condition';
$rs = $conn->Execute($sql);
if($rs === false)
{
    trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $conn->ErrorMsg(), E_USER_ERROR);
}
else
{
    $rows_returned = $rs->RecordCount();
}
$rs->MoveFirst();
while (!$rs->EOF)
{
    print $rs->fields['col1_name'].' '.$rs->fields['col2_name'].'<br>';
    $rs->MoveNext();
}
//to move to recordset of specific number (example: 10), use:
//    $rs->Move(10);
//to limit number of recordset (example: return 10 records with offset 3), use
//    $rs=$conn->SelectLimit($sql,10,3);

    //example:
    $sql = "SELECT newsId, title, content, datelastupdate, datecreated, user FROM news;";
    $rs = $conn->Execute($sql);
    if($rs === false)
    {
        trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $conn->ErrorMsg(), E_USER_ERROR);
    }
    else
    {
        $rows_returned = $rs->RecordCount();
        if($rows_returned == 0)
        {
            echo "no news recorded in database";
        }
        else
        {
            $rs->MoveFirst();
            echo "
            <table border=1>
                <tr>
                    <th>ID</th>
                    <th>title</th>
                    <th>content</th>
                    <th>last update</th>
                    <th>created</th>
                    <th>user</th>
                    <th>menu</th>
                </tr>
            ";
            while (!$rs->EOF)
            {
                echo "
                <tr>
                    <td>" . $rs->fields['newsId'] . "</td>
                    <td>" . $rs->fields['title'] . "</td>
                    <td>" . $rs->fields['content'] . "</td>
                    <td>" . $rs->fields['datelastupdate'] . "</td>
                    <td>" . $rs->fields['datecreated'] . "</td>
                    <td>" . $rs->fields['user'] . "</td>
                </tr>
                ";
                $rs->MoveNext();
            }
            echo "
            </table>
            ";
        }
    }
```

## HOW TO USE: (I) INSERT
```php
<?php
$value_column1 = $conn->qstr('col1_value');
$sql = "INSERT INTO tbl (col1_varchar, col2_int) VALUES ($value_column1, 1)";
if($conn->Execute($sql) === false)
{
    trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $conn->ErrorMsg(), E_USER_ERROR);
}
else
{
    $last_inserted_id = $conn->Insert_ID();
    $affected_rows = $conn->Affected_Rows();
}

    //example:
    $value_title = $conn->qstr("BIG NEWS!!");
    $value_content = $conn->qstr("A man found dead at local trash can, the local authorities tried to find the culprit.");
    $value_user = $conn->qstr("John Doe");
    $sql = "
        INSERT INTO news (title, content, datelastupdate, datecreated, user)
        VALUES ($value_title, $value_content, CURDATE(), CURDATE(), $value_user)
        ";
    if($conn->Execute($sql) === false)
    {
        trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $conn->ErrorMsg(), E_USER_ERROR);
    }
    else
    {
        $last_inserted_id = $conn->Insert_ID();
        $affected_rows = $conn->Affected_Rows();
        echo "new news added successfully";
    }
```

## HOW TO USE: (U) UPDATE
```php
<?php
$value_column1 = $conn->qstr('col1_value');
$sql = "UPDATE tbl SET col1_varchar=$value_column1, col2_int=1 WHERE condition";
if($conn->Execute($sql) === false)
{
    trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $conn->ErrorMsg(), E_USER_ERROR);
}
else
{
    $affected_rows = $conn->Affected_Rows();
}

    //example:
    $input_newsId = 3;
    $value_title = $conn->qstr("BIG NEWS!!");
    $value_content = $conn->qstr("A man found dead at local trash can, the local authorities tried to find the culprit.");
    $value_user = $conn->qstr("John Doe");
    $sql = "
        UPDATE news
        SET
            title=$value_title,
            content=$value_content,
            datelastupdate=CURDATE(),
            user=$value_user
        WHERE newsId=$input_newsId;
        ";
    if($conn->Execute($sql) === false)
    {
        trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $conn->ErrorMsg(), E_USER_ERROR);
    }
    else
    {
        $affected_rows = $conn->Affected_Rows();
        echo "news edited successfully";
    }
```

## HOW TO USE: (D) DELETE
```php
<?php
$sql = "DELETE FROM tbl WHERE condition";
if($conn->Execute($sql) === false)
{
    trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $conn->ErrorMsg(), E_USER_ERROR);
}
else
{
    $affected_rows = $conn->Affected_Rows();
}

    //example:
    $deleteTargetNewsId = $_GET['deleteNewsId'];
    $sql="DELETE FROM news WHERE newsId=$deleteTargetNewsId";
    if($conn->Execute($sql) === false)
    {
        trigger_error('Wrong SQL: ' . $sql . ' Error: ' . $conn->ErrorMsg(), E_USER_ERROR);
    }
    else
    {
        $affected_rows = $conn->Affected_Rows();
        echo "news with id: " . $deleteTargetNewsId . " deleted successfully";
    }
```

## OTHER NOTE: SANITIZE AGAINST SQL INJECTION (THIS IS CALLED 'PREPARED STATEMENT')
TAKEN FROM http://bobby-tables.com/php
```php
<?php
$dbConnection = NewADOConnection($connectionString);
$sqlResult = $dbConnection->Execute(
    'SELECT user_id,first_name,last_name FROM users WHERE username=? AND password=?',
    array($_POST['username'], sha1($_POST['password'])
);
```

## OTHER NOTE: TRANSACTION

- TRANSACTION IS USED FOR MULTIPLE QUERIES THAT MUST BE COMPLETED SUCCESSFULLY TOGETHER
- EXAMPLE OF TRANSACTION: membuat nota beli, lalu mengupdate jumlah barang di stok barang. jika salah satu query gagal, maka seluruh query harus di-rollback
```php
<?php
$conn = NewADOConnection($connectionString);
$conn->StartTrans();
$conn->Execute($sql1);
$conn->Execute($Sql2);
$conn->CompleteTrans();
//use $conn->FailTrans(); to rollback/declare the transaction as failed

    //example:
    $conn->StartTrans();
    $sql1_unprepared = "INSERT INTO nota_beli (tanggal, BarangID, SupplierID, jumlah) VALUES ( CURDATE(), ?, ?, ? )";
    $sql1 = $conn->Prepare($sql1_unprepared);
    $conn->Execute($sql1, array($idbarang, $idsupplier, $jumlah_beli) );
    $sql2_unprepared = "UPDATE barang SET stok = stok + ? WHERE BarangID = ?";
    $sql2 = $conn->Prepare($sql2_unprepared);
    $conn->Execute($Sql2, array($jumlah_beli, $idbarang) );
    $conn->CompleteTrans();
```

## OTHER NOTE: USE ADODB TO CONNECT TO POSTGRESQL
```php
<?php
$connPostgre = ADONewConnection("postgres");
//$connPostgre->debug = true; //SET TO TRUE TO MAKE EVERY QUERY AND RESPONSE VISIBLE
$DBServerPostgre = 'localhost';
$DBUserPostgre = 'postgresql_username';
$DBPassPostgre = 'postgresql_password';
$DBNamePostgre = 'postgresql_database';
$connPostgre->setFetchMode(ADODB_FETCH_ASSOC); // fetch mode can be ADODB_FETCH_ASSOC or ADODB_FETCH_NUM
$connPostgre->Connect($DBServerPostgre, $DBUserPostgre, $DBPassPostgre, $DBNamePostgre);
//CAUTION! POSTGRESQL SQL QUERY FORMAT IS A BIT DIFFERENT FROM MYSQL SQL QUERY FORMAT
```

### EXAMPLE:
```php
<?php
//MYSQL: SELECT namasatker FROM ms_satker WHERE idsatker = "s503573" ;
$sql = 'SELECT namasatker FROM ms_satker WHERE idsatker = "s503573" ;';
$conn->Execute($sql);
//IF THE VALUE IS NUMERIC, THERE IS NO NEED TO QUOTE THEM
//POSTGRESQL: SELECT "namasatker" FROM "ms_satker" WHERE "idsatker" = 's503573' ;
$sql = 'SELECT "namasatker" FROM "ms_satker" WHERE "idsatker" = ? ;';
$sql = $conn->Prepare($sql);
$rs = $conn->Execute($sql, array('s503573'));
```
- THE QUOTATION TO QUOTE VALUE HAVE TO BE SINGLE QUOTE
- THE QUOTATION TO QUOTE FIELDNAME HAVE TO BE DOUBLE QUOTE
- EVEN IF THE VALUE IS NUMERIC IT HAVE TO BE QUOTED
- TO RESOLVE THE QUOTATION ISSUE, JUST LEAVE THE VALUE TO PREPARED STATEMENT LIKE THE EXAMPLE ABOVE



## OTHER NOTE: USE ADODB TO CONNECT TO MICROSOFT SQL SERVER
```php
<?php
$conn = &ADONewConnection('mssqlnative');
$conn->setFetchMode(ADODB_FETCH_ASSOC);
$connectResult = $conn->Connect('np:\\\\.\pipe\LOCALDB#6E8B6BFB\tsql\query', '', '', 'NamaDatabase1');

$query = "SELECT * FROM mytable";
$rs = $conn->execute($query);
$arr = $rs->GetArray();
print_r($arr);
```
