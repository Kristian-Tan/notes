    #include <stdio.h>
    #include <stdlib.h>
    #include <sys/types.h>
    #include <unistd.h>

    int main()
    {
        clearenv();
        setuid(1005);
        setreuid(1005, 1005);
        system("/home/kristian/setuid_script.sh");
        return 0;
    }

