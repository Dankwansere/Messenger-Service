package com.sans.model;

/***
 * Eric Sarpong
 * Simple Chat messenger 
 * 
 * Description: This web service will allow different applications on different
 * platform to send messages to each other real time
 * 
 * Date: August 20th 2015
 * 
 */

import com.sans.iFace.*;

import java.util.ArrayList;

import com.sans.model.userMessage;



public class messengerWebService implements Imessage {
	
	//Array list to hold users logged into system
	private static ArrayList<String> userPool = new ArrayList<String>();
	private static ArrayList<userMessage> messagePool = new ArrayList<userMessage>();
	private userMessage message;
	
	
	/*
	 * (non-Javadoc)
	 * @see com.sans.iFace.Imessage#addUser(java.lang.String)
	 * 
	 * To add user. Will not add user if user already exist in system
	 */
	@Override
	public String addUser(String userName)
	{
		//boolean result = true;
		String responseMessage = "";
		boolean addUser = true;
	
		if(userPool.size() == 0)
		{
			userPool.add(userName);
			responseMessage = userName + " has successfully logged in";	
			System.out.println(userName + " has successfully logged in");
			addUser = false;
		}
		
		else
		{

			for(int i = 0; i < userPool.size(); i++)
			{
				if(userName.equals(userPool.get(i).toString()))
				{
					System.out.println("User is already logged in");
					addUser = false;
				}			
			}
		}
		
		if(addUser == true)
		{
			userPool.add(userName);
			responseMessage = userName + " has successfully logged in";	
			System.out.println(userName + " has successfully logged in");
		}

		return responseMessage;
	}

	/*
	 * (non-Javadoc)
	 * @see com.sans.iFace.Imessage#getUsers()
	 * 
	 * return a list of users currently logged into application
	 */
	@Override
	public String[] getUsers() {
		
		String[] userList = new String[userPool.size()];
		
		for(int i = 0; i < userList.length; i++)
		{
			userList[i] = userPool.get(i);
		}
				
		return userList;
	}

	/*
	 * (non-Javadoc)
	 * @see com.sans.iFace.Imessage#sendMessage(java.lang.String, java.lang.String, java.lang.String)
	 * 
	 * Send message to user
	 * Param toUser: Destination user
	 * Param fromUser: Source/Sender user
	 * Param message: Message
	 */
	@Override
	public boolean sendMessage(String toUser, String fromUser, String message) {
		
		boolean result = true;
		
		this.message = new userMessage(toUser, fromUser, message);
		messagePool.add(this.message);
		
		return result;
	}
	
	/*
	 * (non-Javadoc)
	 * 
	 * Retrieve message from message pool
	 */
	public String[] retrieveMessage(String userName)
	{
		String message = "";
		String fromUser = "";
		String[] responseMessage = new String[2];
		
		
		for(int i = 0; i < messagePool.size(); i++)
		{
			if(userName.equals(messagePool.get(i).getToUser()))
			{
				message = messagePool.get(i).getMessage();
				fromUser = messagePool.get(i).getFromUser();
				responseMessage[0] = message;
				responseMessage[1] = fromUser;
				messagePool.remove(i);		
			}
		}
		
		return responseMessage;
	}

	/*
	 * (non-Javadoc)
	 * @see com.sans.iFace.Imessage#removeUser(java.lang.String)
	 * 
	 * Remove user from user pool
	 */
	@Override
	public boolean removeUser(String userName) {
		
		boolean response = false;
		
		for(int i = 0; i < userPool.size(); i++)
		{
			if(userName.equals(userPool.get(i)))
			{
				userPool.remove(i);
				
				response = true;
			}
			else
			{
				response = false;
			}
		}
		
		return response;
	}
	
}