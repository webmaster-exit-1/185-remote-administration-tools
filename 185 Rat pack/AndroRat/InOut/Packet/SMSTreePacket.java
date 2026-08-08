package Packet;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.ArrayList;

public class SMSTreePacket implements Packet{

	ArrayList<SMSPacket> list;

	public SMSTreePacket() {
		
	}
	
	public SMSTreePacket(ArrayList<SMSPacket> ar) {
		list = ar;
	}
	
	public byte[] build() {
		try {
			ByteArrayOutputStream bos = new ByteArrayOutputStream();
			ObjectOutputStream out = new ObjectOutputStream(bos);
			out.writeObject(list);
			return bos.toByteArray();
		} catch (IOException e) {
			return null;
		}
	}

	public void parse(byte[] packet) {
		ByteArrayInputStream bis = new ByteArrayInputStream(packet);
		ObjectInputStream in;
		try {
			in = new ObjectInputStream(bis);
			Object object = in.readObject();
			if (!(object instanceof ArrayList<?>)) {
				throw new IOException("Invalid SMS packet list");
			}
			ArrayList<?> parsedList = (ArrayList<?>) object;
			ArrayList<SMSPacket> smsList = new ArrayList<>();
			for (Object item : parsedList) {
				if (!(item instanceof SMSPacket)) {
					throw new IOException("Invalid SMS packet");
				}
				smsList.add((SMSPacket) item);
			}
			list = smsList;
		} catch (Exception e) {
		}
	}

	public ArrayList<SMSPacket> getList() {
		return list;
	}
}