package com.sans.iFace;


public interface Imessage {
	
	//To add user
	public String addUser(String userName);
	
	//To get list of users
	public String[] getUsers();
	
	//To send message
	public boolean sendMessage(String toUser, String fromUser, String message);
	
	public boolean removeUser(String userName);
	
	

}
