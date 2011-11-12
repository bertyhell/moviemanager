package logic.common;

import java.text.DecimalFormat;

/**
 *
 * @author Alexander
 */
public class TimeFormatter {
    

    public static String longToTime(long time) {
	long seconds = time / 1000;
	long minutes = seconds / 60;
	long hours = (int) Math.floor((double) (minutes / 60));
	minutes = minutes - hours * 60;
	seconds = seconds - minutes * 60 - hours * 60 * 60;
	DecimalFormat myFormat = new DecimalFormat("00");
	return hours + ":" + myFormat.format(minutes) + ":" + myFormat.format(seconds);
    }
}
