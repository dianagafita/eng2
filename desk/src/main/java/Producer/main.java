package Producer;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;
import org.json.JSONObject;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.io.File;
import java.nio.charset.StandardCharsets;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.Month;
import java.util.Scanner;


@SpringBootApplication
public class main {
    public static void main(String[] args) throws Exception {
        SpringApplication.run(main.class, args);
        String uri = System.getenv("CLOUDAMQP_URL");
        if (uri == null) uri = "amqps://lqdhszah:FsyKTNh5FYyVMShPAfce0d5RTC4L9ROb@sparrow.rmq.cloudamqp.com/lqdhszah";

        ConnectionFactory factory = new ConnectionFactory();
        factory.setUri(uri);

        //Recommended settings
        factory.setConnectionTimeout(30000);

        Connection connection = factory.newConnection();
        Channel channel = connection.createChannel();

        String queue = "Sensor_simulator";     //queue name
        boolean durable = false;    //durable - RabbitMQ will never lose the queue if a crash occurs
        boolean exclusive = false;  //exclusive - if queue only will be used by one connection
        boolean autoDelete = false; //autodelete - queue is deleted when last consumer unsubscribes

        channel.queueDeclare(queue, durable, exclusive, autoDelete, null);


        String exchangeName = "";
        String routingKey = "Sensor_simulator";

        LocalDateTime date = LocalDateTime.of(2021,12,20,00,0);
        File file = new File("C:\\Users\\User\\Desktop\\AN 4-SEM1\\SD\\desk\\src\\main\\resources\\sensor.csv");
        Scanner scan = new Scanner(file);
        while (scan.hasNextLine()) {
            //System.out.println(scan.nextLine());
            String value = scan.nextLine();
            System.out.println(value);
            JSONObject obj = new JSONObject();

            date = date.plusMinutes(1000);
            obj.put("device_id", "5c2494a3-1140-4c7a-991a-a1a2561c6bc2");
            obj.put("timestamp", date);
            obj.put("value", value);


            System.out.println(obj);
            channel.basicPublish(exchangeName, routingKey, null, obj.toString().getBytes(StandardCharsets.UTF_8));

            Thread.sleep(500);
        }
    }

}
