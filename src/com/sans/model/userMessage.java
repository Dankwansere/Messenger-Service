package com.sans.model;

public class userMessage {
	
	private String toUser;
	private String fromUser;
	private String message;
	
	
	public userMessage(String toUser, String fromUser, String message)
	{
		this.toUser = toUser;
		this.fromUser = fromUser;
		this.message = message;
	}


	public String getToUser() {
		return toUser;
	}


	public void setToUser(String toUser) {
		this.toUser = toUser;
	}


	public String getFromUser() {
		return fromUser;
	}


	public void setFromUser(String fromUser) {
		this.fromUser = fromUser;
	}


	public String getMessage() {
		return message;
	}


	public void setMessage(String message) {
		this.message = message;
	}

}
