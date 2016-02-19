package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;

import java.io.FileInputStream;
import java.io.IOException;
import java.util.Properties;

import org.junit.Test;

;

public class PropertyTest {
	@Test
	public void testReadProperty() throws IOException {
		Properties props = new Properties();
		props.load(new FileInputStream("resources/test.properties"));

		assertEquals("cheka", props.getProperty("name"));
		assertEquals("tsinghua", props.getProperty("university"));
	}
}
