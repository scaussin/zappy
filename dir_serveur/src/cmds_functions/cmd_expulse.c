#include "../../includes/serveur.h"

void	cmd_expulse(t_serveur *serv, t_client_entity *client_cur, char *param)
{
	(void)					param;
	t_player				*cur_player;
	t_client_entity			*list_clients;
	char					*msg;
	int						nb_client_expulsed;

	nb_client_expulsed = 0;
	cur_player = &(client_cur->player);
	list_clients = serv->client_hdl.list_clients;

	asprintf(&msg, "pex #%d\n",client_cur->sock);
	push_gfx_msg(serv, msg);
	free(msg);

	// bouger les client qui sont sur la meme case que client_cur
	while (list_clients)
	{
		if (list_clients->is_in_game == 1
			&& list_clients->is_gfx == 0
			&& list_clients->is_disconnecting == 0
			&& list_clients->player.pos.x == cur_player->pos.x
			&& list_clients->player.pos.y == cur_player->pos.y
			&& &(list_clients->player) != cur_player)
		{
			expulse_client_in_dir(serv, list_clients, cur_player->dir);
			nb_client_expulsed++;
		}	
		list_clients = list_clients->next;
	}

	if (nb_client_expulsed == 0)
		write_buffer(&client_cur->buff_send, "ko\n", 3);
	else
		write_buffer(&client_cur->buff_send, "ok\n", 3);
}

void	expulse_client_in_dir(t_serveur *serv, t_client_entity *client, int dir)
{
	t_player	*player;
	char		*msg;
	char		*msg_client;

	player = &(client->player);
	if (dir == UP) // 0 north
		player->pos.y += 1;
	else if (dir == RIGHT) // 1 east
		player->pos.x += 1;
	else if (dir == DOWN) // 2 south
		player->pos.y -= 1;
	else if (dir == LEFT) // 3 west
		player->pos.x -= 1;
	// Boundary check
	if (player->pos.x >= serv->world_hdl.map_x)
		client->player.pos.x = 0;
	else if (player->pos.x < 0)
		client->player.pos.x = serv->world_hdl.map_x - 1;
	if (player->pos.y >= serv->world_hdl.map_y)
		client->player.pos.y = 0;
	else if (player->pos.y < 0)
		client->player.pos.y = serv->world_hdl.map_y - 1;
	// Case assignation
	player->cur_case = &(serv->world_hdl.
			world_board[player->pos.y][player->pos.x]);

	// player client response.
	asprintf(&msg_client, "deplacement %d\n",dir + 1);
	write_buffer(&client->buff_send, msg_client, strlen(msg_client));
	free(msg_client);

	// gfx msg : "ppo #n X Y O\n" 
	asprintf(&msg, "ppo #%d %d %d %d\n",
		client->sock,
		player->pos.x,
		player->pos.y,
		player->dir + 1);
	push_gfx_msg(serv, msg);
	free(msg);
}
