package wsu.cheka.basictest;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.Random;

import org.junit.Before;
import org.junit.Test;

public class StreamTest {
	private static final int BUFFER_LEN = 1024;
	private byte[] m_srcbuffer;
	private byte[] m_cpybuffer;

	@Before
	public void setUp() {
		m_srcbuffer = new byte[BUFFER_LEN];
		m_cpybuffer = new byte[BUFFER_LEN];

		Random random = new Random(System.currentTimeMillis());
		random.nextBytes(m_srcbuffer);
	}

	@Test
	public void testFileIOStream() throws IOException {
		// write
		String filename = "resources/testfileio.dat";
		FileOutputStream outstream = new FileOutputStream(filename);
		try {
			outstream.write(m_srcbuffer);
		} finally {
			outstream.close();
		}

		// read out
		FileInputStream inStream = new FileInputStream(filename);
		int size;
		try {
			size = inStream.read(m_cpybuffer);
			assertEquals(-1, inStream.read());// assert the end is met
		} finally {
			inStream.close();
		}

		// check result
		assertEquals(BUFFER_LEN, size);
		assertArrayEquals(m_srcbuffer, m_cpybuffer);
	}

	@Test
	public void testBufferIOStream() throws IOException {
		String filename = "resources/testbufferio.dat";

		// output data to file
		BufferedOutputStream outStream = new BufferedOutputStream(
				new FileOutputStream(filename));
		for (byte data : m_srcbuffer) {
			outStream.write(data);
		}
		outStream.flush();

		// read data from file
		BufferedInputStream inStream = new BufferedInputStream(
				new FileInputStream(filename));
		int index = 0;
		int readdata = inStream.read();
		while (readdata != -1) {
			m_cpybuffer[index] = (byte) readdata;
			++index;
			readdata = inStream.read();
		}

		outStream.close();
		inStream.close();

		assertEquals(BUFFER_LEN, index);
		assertArrayEquals(m_srcbuffer, m_cpybuffer);
	}

	@Test
	public void testDataIOStream() throws IOException {
		ByteArrayOutputStream bytearrOutStream = new ByteArrayOutputStream();
		DataOutputStream dataOutstream = new DataOutputStream(bytearrOutStream);

		Random random = new Random();
		int srcInt = random.nextInt();
		float srcFloat = random.nextFloat();
		String srcString = "hello cheka from WSU";

		// write content into stream
		dataOutstream.writeInt(srcInt);
		dataOutstream.writeFloat(srcFloat);
		dataOutstream.writeUTF(srcString);
		dataOutstream.flush();

		// read content out of stream
		ByteArrayInputStream bytearrInStream = new ByteArrayInputStream(
				bytearrOutStream.toByteArray());
		DataInputStream dataInstream = new DataInputStream(bytearrInStream);

		int cpyInt = dataInstream.readInt();
		assertEquals(srcInt, cpyInt);

		float cpyFloat = dataInstream.readFloat();
		assertEquals(srcFloat, cpyFloat, 1e-6);

		String cpyString = dataInstream.readUTF();
		assertEquals(srcString, cpyString);

		dataOutstream.close();
		dataInstream.close();
	}

	@Test
	public void testObjectIOStream() throws IOException, ClassNotFoundException {
		StudentInfo[] srcStudentsInfo = { new StudentInfo("cheka", 1),
				new StudentInfo("henry", 2) };

		// write contents into stream
		ByteArrayOutputStream bytearrOutstream = new ByteArrayOutputStream();
		ObjectOutputStream objoutstream = new ObjectOutputStream(
				bytearrOutstream);

		for (StudentInfo stdinfo : srcStudentsInfo) {
			objoutstream.writeObject(stdinfo);
		}

		// read contents out of stream
		ByteArrayInputStream bytearrInstream = new ByteArrayInputStream(
				bytearrOutstream.toByteArray());
		ObjectInputStream objinStream = new ObjectInputStream(bytearrInstream);

		StudentInfo cpyStudInfo = null;
		for (int index = 0; index < srcStudentsInfo.length; ++index) {
			// readObject will throw ClassNotFoundException
			cpyStudInfo = (StudentInfo) objinStream.readObject();
			assertEquals(srcStudentsInfo[index], cpyStudInfo);
		}

		assertEquals(-1, objinStream.read());// it must reach the end of the
												// stream

		objoutstream.close();
		objinStream.close();
	}
}
