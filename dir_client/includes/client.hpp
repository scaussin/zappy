#ifndef CLIENT_HPP
# define CLIENT_HPP

// General includes
# include <unistd.h>
# include <stdlib.h>
# include <iostream>
# include <sstream>
# include <string>
# include <stdio.h>
# include <regex>
# include <errno.h>
# include <vector>
# include <list>

// Connection includes.
# include <sys/socket.h>
# include <arpa/inet.h>
# include <netdb.h>
# include <fcntl.h>
# include <sys/time.h>

# define BUFF_SIZE 128
# define END "\n"
# define LEN_END 1
/*
// Connection related structure.
typedef struct			s_CommunicationSettings
{
	std::string				TeamName;
	int						Port;
	std::string				HostName;


}							t_CommunicationSettings;*/

// Class definitions.
class ClientInputHandler; // handle and check args.
class ClientCommunication; // communicate with the server.
class ClientPlayer; // the actual game player.
class ClientBuffer;
class ClientController; // main controller.


// Class files includes.
# include "CustomException.hpp"
# include "ClientInputHandler.hpp"
# include "ClientCommunication.hpp"
# include "ClientPlayer.hpp"
# include "ClientBuffer.hpp"
# include "ClientController.hpp"


// color in text;
# define KNRM  "\x1B[0m"
# define KRED  "\x1B[31m"
# define KGRN  "\x1B[32m"
# define KYEL  "\x1B[33m"
# define KBLU  "\x1B[34m"
# define KMAG  "\x1B[35m"
# define KCYN  "\x1B[36m"
# define KWHT  "\x1B[37m"
# define KRESET "\x1B[0m"

#endif
