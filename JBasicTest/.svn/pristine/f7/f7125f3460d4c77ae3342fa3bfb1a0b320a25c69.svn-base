package wsu.cheka.basictest;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;

import org.junit.Before;
import org.junit.Test;

public class EnumTest {
	enum DIRECTION {
		NORTH, EAST, WEST, SOUTH
	}

	String[] m_strDescriptions;

	@Before
	public void setUp() {
		m_strDescriptions = new String[] { "NORTH", "EAST", "WEST", "SOUTH" };
	}

	@Test
	public void testIterate() {
		String[] expectedStrings = new String[4];

		int index = 0;
		for (DIRECTION value : DIRECTION.values()) {
			expectedStrings[index] = value.toString();
			++index;
		}

		assertArrayEquals(expectedStrings, m_strDescriptions);
	}

	@Test
	public void testValueof() {
		DIRECTION[] directions = { DIRECTION.NORTH, DIRECTION.EAST,
				DIRECTION.WEST, DIRECTION.SOUTH };

		for (int index = 0; index < m_strDescriptions.length; ++index) {
			assertEquals(directions[index], DIRECTION
					.valueOf(m_strDescriptions[index]));
		}
	}

	@Test
	public void testOridinal() {
		int index = 0;
		DIRECTION[] enumValues = DIRECTION.values();
		for (DIRECTION value : enumValues) {
			assertEquals(index, value.ordinal());
			assertEquals(value, enumValues[index]);
			++index;
		}
	}
}
