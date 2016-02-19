package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;

import org.junit.Test;

public class StringBuilderTest {
	@Test
	public void testLength() {
		String item = "cheka";
		StringBuilder builder = new StringBuilder(item);

		assertEquals(builder.length(), item.length());
	}

	@Test
	public void testTruncate() {
		StringBuilder strbuilder = new StringBuilder("cheka");
		strbuilder.setLength(3);
		assertEquals("che", strbuilder.toString());
	}
	
	@Test
	public void testAppend()
	{
		StringBuilder builder = new StringBuilder();
		builder.append("cheka");
		builder.append(1);
		builder.append(true);
		
		assertEquals("cheka1true", builder.toString());
	}
}
