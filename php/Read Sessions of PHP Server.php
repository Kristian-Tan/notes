<?php

// this script is for finding out php active sessions
// you must run this script in command line / terminal AS root / superuser

// basic concept:
// php saved its sessions in files in a certain directory
// to show the name of that directory: open phpinfo, see where session is stored (default to /var/lib/php/sessions OR just call echo session_save_path() ) and how long is session lifetime
// not all sessions there is active, use phpinfo to see how long sessions will be retained
// filesystem in linux will attach 3 kinds of timestamp to each file (and directory): 
//   - a (access time/last read),
//   - m (modified time/last write),
//   - c (changed time/last attribute change like permission)
// it is possible that the OS did not save access time by specifying "noatime" in boot option or in fstab
// to do a search of file based on it's atime, use ```find . -atime -1``` which means ```find``` any file in current directory ```.``` that have its ```atime```` is less than ```-``` (minus) ```1``` (1x24 hours) ago


// set session id to "root" so that it won't continously add new session for each single cli call
session_id("root");
// start session: this is needed for calling session_decode()
session_start();

// get the location of session files, it should defaults to "/var/lib/php/sessions"
$session_save_path = session_save_path();

// change working directory to that directory
chdir($session_save_path);

// get files that atime (access time) is less than (minus sign) 1x24 hours ago
$result = shell_exec("find . -atime -1");

// show list of sessions found
echo "found sessions: \r\n";
print_r($result);

// the result of shell_exec is multiline string, output of the command
if(is_string($result)) {
    $result = str_replace("\r", "", $result);
    $result = explode("\n", $result);
}

// loop for each files that was found from above command
$count = 0;
foreach ($result as $key => $value) {

    // show session name
    echo "session: ".$value." => ";

    // if the file exist (and not a directory)
    if( file_exists($value) && !is_dir($value) ) {

        // read content of the file
        $content = file_get_contents($value);

        // decode session content
        $data = session_decode($content);

        // show session content
        print_r($_SESSION);
        $count++;
    }
}
echo "\r\nprinted $count sessions\r\n";
