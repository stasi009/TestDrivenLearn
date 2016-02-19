package wsu.cheka.basictest;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;

import java.io.BufferedReader;
import java.io.CharArrayReader;
import java.io.CharArrayWriter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.List;

import org.junit.Test;

public class ReaderWriterTest {
	@Test
	public void testIOStreamRW() throws IOException {
		String filename = "resources/testfilerw.txt";
		String writeMessage = "hello cheka from WSU";

		OutputStreamWriter writer = new OutputStreamWriter(
				new FileOutputStream(filename));
		try {
			writer.write(writeMessage);
		} finally {
			writer.close();
		}

		char[] readChars = new char[128];
		InputStreamReader reader = new InputStreamReader(new FileInputStream(
				filename));
		int numChars;
		try {
			numChars = reader.read(readChars);
		} finally {
			reader.close();
		}
		String readMessage = String.valueOf(readChars, 0, numChars);

		assertEquals(writeMessage, readMessage);
	}

	@Test
	public void testFileReaderWriter() throws IOException {
		String filename = "resources/testfilerw.txt";
		String writeMsg = "hello Java from Cheka in WSU";

		FileWriter fWriter = new FileWriter(filename);
		try {
			fWriter.write(writeMsg);
		} finally {
			fWriter.close();
		}

		FileReader fReader = new FileReader(filename);
		char[] charbuffer = new char[128];
		int numchars;
		try {
			numchars = fReader.read(charbuffer);
		} finally {
			fReader.close();
		}

		assertEquals(numchars, writeMsg.length());
		String readMsg = String.valueOf(charbuffer, 0, numchars);
		assertEquals(writeMsg, readMsg);
	}

	@Test
	public void testLineBasedRW() throws IOException {
		String filename = "resources/testlinerw.txt";
		String[] srcMessage = { "hello cheka!", "Hello Java!!", "Hello WSU!!!",
				"Hello USA Life!!!!" };

		// write contents to file
		PrintWriter printer = new PrintWriter(new FileWriter(filename));
		for (String msg : srcMessage) {
			printer.println(msg);
		}
		printer.close();

		// read contents out of file
		List<String> readMsgList = new ArrayList<String>();
		BufferedReader bufreader = new BufferedReader(new FileReader(filename));
		String readmsg;
		while ((readmsg = bufreader.readLine()) != null) {
			readMsgList.add(readmsg);
		}
		bufreader.close();

		assertEquals(srcMessage.length, readMsgList.size());

		String[] readMsgArray = new String[readMsgList.size()];
		readMsgList.toArray(readMsgArray);
		assertArrayEquals(srcMessage, readMsgArray);
	}

	@Test
	public void testCharArrayRW() throws IOException {
		CharArrayWriter carrWriter = new CharArrayWriter();
		String srcMsg = "hello cheka from wsu in java!!";
		carrWriter.write(srcMsg);
		carrWriter.close();

		CharArrayReader carrReader = new CharArrayReader(carrWriter
				.toCharArray());
		char[] charArray = new char[128];
		int numRead = carrReader.read(charArray);
		carrReader.close();

		String readMsg = String.valueOf(charArray, 0, numRead);
		assertEquals(srcMsg, readMsg);
	}
}
