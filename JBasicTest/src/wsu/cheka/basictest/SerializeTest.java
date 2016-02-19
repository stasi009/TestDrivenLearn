package wsu.cheka.basictest;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.Random;

import org.junit.*;
import static org.junit.Assert.*;

public class SerializeTest 
{
	private static class TestSerializeObj implements Serializable
	{
		private int m_serialField;
		private transient int m_nonSerialField;
		
		public TestSerializeObj(int serialValue,int nonSerialValue)
		{
			m_serialField = serialValue;
			m_nonSerialField = nonSerialValue;
		}

		public int getSerialField() {
			return m_serialField;
		}

		public int getNonSerialField() {
			return m_nonSerialField;
		}
	}
	
	private Object copyBySerialize(Object srcObj) throws IOException,ClassNotFoundException
	{
		ByteArrayOutputStream byteOutStream = new ByteArrayOutputStream();
		ObjectOutputStream objOutputStream = new ObjectOutputStream(byteOutStream);
		objOutputStream.writeObject(srcObj);
		
		ByteArrayInputStream byteInStream = new ByteArrayInputStream(byteOutStream.toByteArray());
		ObjectInputStream objInputStream = new ObjectInputStream(byteInStream);
		return objInputStream.readObject();
	}

	@Test
	public void testTransientField() throws IOException,ClassNotFoundException
	{
		Random random = new Random();
		
		TestSerializeObj srcobj = new TestSerializeObj(random.nextInt(100)+100,random.nextInt(100) + 200);
		TestSerializeObj cpyobj = (TestSerializeObj)copyBySerialize(srcobj);
		
		assertEquals(srcobj.getSerialField(), cpyobj.getSerialField());
		assertEquals(0, cpyobj.getNonSerialField());// not serialized
	}
}
