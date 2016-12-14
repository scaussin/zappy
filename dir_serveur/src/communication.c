#include "../includes/serveur.h"

void ckeck_all_clients_communication(t_server_data *server, fd_set *read_fs)
{
  int i;
  int ret_read;

  i = -1;
  while (++i < server->nb_clients)
  {
    if (FD_ISSET(server->list_clients[i].sock, read_fs))
    {
      ret_read = read_client(server->list_clients[i].sock);
      if (ret_read == 0)
      {
        disconnect_client(server, i);
        return ;
      }
    }
  }
}

int   read_client(t_client *client)
{
  int   ret;

  ret = 0;
  if ((ret = recv(c_sock, buffer, BUFF_SIZE - 1, 0)) < 0)
  {
    perror("recv()");
    ret = 0;
  }
  buffer[ret] = '\0';
  printf("Recu : %s\n", buffer);
  return(ret);
}

void new_connection_communication(t_client *client)
{
  char  buff_send[BUFF_SIZE];
  char  buff_recv[BUFF_SIZE];
  int   ret;

  memset(buff_send, 0, BUFF_SIZE);
  strcpy(buff_send, "BIENVENUE\n");
  if (send(client->sock, buff_send, strlen(buff_send), 0) < 0)
    exit_error("send()");
  printf("Send : %s\n", buff_send);

  if ((ret = recv(client->sock, buff_recv, BUFF_SIZE - 1, 0)) < 0)
    exit_error("recv()");
  buff_recv[ret] = '\0';
  strcpy(client->team, buff_recv);
  printf("Recu : %s\n", buff_recv);

  memset(buff_send, 0, BUFF_SIZE);
  strcpy(buff_send, "10\n");
  if (send(client->sock, buff_send, strlen(buff_send), 0) < 0)
    exit_error("send()");
  printf("Send : %s\n", buff_send);

  memset(buff_send, 0, BUFF_SIZE);
  strcpy(buff_send, "20 20\n");
  if (send(client->sock, buff_send, strlen(buff_send), 0) < 0)
    exit_error("send()");
  printf("Send : %s\n", buff_send);
}
