<?php
// Induzy Functions

//Mailchimp Config
$mailchimp_api = 'bb8d6b38a22cca8af94d04a0c74dd5b2-us19';
$mailchimp_list_id = 'c0ac157f49';

//E-mail Config
$recipient_mail = "abileweb8@gmail.com";
$recipient_name = "Abile Web";

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset( $_POST['mcemail'] ) ) {

	$email = filter_var(trim($_POST["mcemail"]), FILTER_SANITIZE_EMAIL);
	$fname = isset( $_POST["fname"] ) && $_POST["fname"] != '' ? $_POST["fname"] : '';
	$lname = isset( $_POST["lname"] ) && $_POST["lname"] != '' ? $_POST["lname"] : '';
	// Check that data was sent to the mailer.
	if ( !filter_var($email, FILTER_VALIDATE_EMAIL)) {
		// Set a 400 (bad request) response code and exit.
		http_response_code(400);
		echo "Oops! There was a problem with your submission. Please complete the form and try again.";
		exit;
	}
	
	$api_key = $mailchimp_api;
	$list_id = $mailchimp_list_id;

	$memberID = md5(strtolower($email));
	
	$data = json_encode( array(
			'email_address' => $email,
			'status' => 'subscribed',
			'merge_fields'  => [
				'FNAME'     => htmlspecialchars( $fname ),
				'LNAME'     => htmlspecialchars( $lname ),
			]		
		)
	);
	
	$url = 'https://' . substr($api_key,strpos($api_key,'-')+1) . '.api.mailchimp.com/3.0/lists/'. $list_id .'/members/'. $memberID;
	$result = rudr_mailchimp_curl_connect( $url, $api_key, $data);
	
	if( isset( $response['response']['code'] ) && $response['response']['code'] == 200 ){
		echo "Already subscribed.";
	} else {
		echo "Thank you, you have been added to our mailing list.";
	}
	
}elseif ($_SERVER["REQUEST_METHOD"] == "POST" && isset( $_POST['g-recaptcha-response'] ) ) {

	// require ReCaptcha class
	require('recaptcha-master/src/autoload.php');

	try {
		// ReCaptch Secret
		$recaptchaSecret = '6LdBHz4UAAAAAEOxNnqwXXzOvBoCUnaai9HZ6c89';

		if (!isset($_POST['g-recaptcha-response'])) {
	        throw new \Exception('ReCaptcha is not set.');
	    }

	    // do not forget to enter your secret key from https://www.google.com/recaptcha/admin
	    
	    $recaptcha = new \ReCaptcha\ReCaptcha($recaptchaSecret, new \ReCaptcha\RequestMethod\CurlPost());
	    
	    // we validate the ReCaptcha field together with the user's IP address
	    
	    $response = $recaptcha->verify($_POST['g-recaptcha-response'], $_SERVER['REMOTE_ADDR']);

	    if (!$response->isSuccess()) {
	        throw new \Exception('ReCaptcha was not validated.');
	    }
	}catch (\Exception $e) {
	    echo "ReCaptcha was not validated.";
		exit;
	}


	// Get the form fields and remove whitespace.
	$name = strip_tags(trim($_POST["name"]));
	$name = str_replace(array("\r","\n"),array(" "," "),$name);
	$email = filter_var(trim($_POST["email"]), FILTER_SANITIZE_EMAIL);
	$phone = trim($_POST["phone"]);
	$subject = trim($_POST["subject"]);
	$message = trim($_POST["message"]);

	// Check that data was sent to the mailer.
	if ( empty($name) OR empty($message) OR !filter_var($email, FILTER_VALIDATE_EMAIL)) {
		// Set a 400 (bad request) response code and exit.
		http_response_code(400);
		echo "Oops! There was a problem with your submission. Please complete the form and try again.";
		exit;
	}
	
	/*$user_file = $userFileType = $userfile_name = '';
	if( isset( $_FILES["userfile"]["tmp_name"] ) ){
		$path = $_FILES['userfile']['name'];
		$userFileType = pathinfo($path, PATHINFO_EXTENSION);
		$allowed_file_types = array( 'pdf', 'doc', 'docx' );
		if( in_array( $userFileType, $allowed_file_types ) ){
			$user_file = $_FILES["userfile"]["tmp_name"];
			$userfile_name = $_FILES["userfile"]["name"];
		}else{
			echo "Oops! There was a problem with your submission. Invalid file type. Upload only pdf, doc or docx";
			exit;
		}
	}*/
	
	require_once "PHPMailer.php";

	$mail = new PHPMailer;
	
	$recipient = $recipient_mail;
	$recipient_name = $recipient_name;

	// Set the email subject.
	$subject = "Induzy contact from $name";

	// Build the email content.
	$email_content = "Name: $name\n";
	$email_content .= "Email: $email\n";
	$email_content .= "Phone: $phone\n";
	$email_content .= "Subject: $subject\n";
	$email_content .= "Message:\n$message\n";
	
	$mail->From = $email;
	$mail->FromName = $name;
	$mail->addAddress($recipient, $recipient_name);
	
	//Provide file path and name of the attachments
	//$mail->addAttachment($user_file, $userfile_name);        
	$mail->isHTML(true);
	$mail->Subject = $subject;
	$mail->Body = nl2br( $email_content );
	//$mail->AltBody = "This is the plain text version of the email content";
	
	if(!$mail->send()){
		echo "Mailer Error: " . $mail->ErrorInfo;
	}else{
		echo "Message has been sent successfully";
	}
	exit;
	

}elseif ($_SERVER["REQUEST_METHOD"] == "POST" && isset( $_POST['services'] ) ) {

	// Get the form fields and remove whitespace.
	$name = strip_tags(trim($_POST["name"]));
	$name = str_replace(array("\r","\n"),array(" "," "),$name);
	$email = filter_var(trim($_POST["email"]), FILTER_SANITIZE_EMAIL);
	$message = trim($_POST["message"]);
	$services = trim($_POST["services"]);

	// Check that data was sent to the mailer.
	if ( empty($name) OR empty($message) OR !filter_var($email, FILTER_VALIDATE_EMAIL)) {
		// Set a 400 (bad request) response code and exit.
		http_response_code(400);
		echo "Oops! There was a problem with your submission. Please complete the form and try again.";
		exit;
	}
		
	require_once "PHPMailer.php";

	$mail = new PHPMailer;
	
	$recipient = $recipient_mail;
	$recipient_name = $recipient_name;

	// Set the email subject.
	$subject = "Induzy contact from $name";

	// Build the email content.
	$email_content = "Name: $name\n";
	$email_content .= "Email: $email\n";
	$email_content .= "Service: $services\n";
	$email_content .= "Message:\n$message\n";
	
	
	$mail->From = $email;
	$mail->FromName = $name;
	$mail->addAddress($recipient, $recipient_name);
	
	//Provide file path and name of the attachments
	//$mail->addAttachment($user_file, $userfile_name);        
	$mail->isHTML(true);
	$mail->Subject = $subject;
	$mail->Body = nl2br( $email_content );
	//$mail->AltBody = "This is the plain text version of the email content";
	
	if(!$mail->send()){
		echo "Mailer Error: " . $mail->ErrorInfo;
	}else{
		echo "Message has been sent successfully";
	}
	exit;
	

} elseif ($_SERVER["REQUEST_METHOD"] == "POST" && isset( $_POST['message'] ) ) {

	// Get the form fields and remove whitespace.
	$name = strip_tags(trim($_POST["name"]));
	$name = str_replace(array("\r","\n"),array(" "," "),$name);
	$email = filter_var(trim($_POST["email"]), FILTER_SANITIZE_EMAIL);
	$phone = trim($_POST["phone"]);
	$subject = trim($_POST["subject"]);
	$message = trim($_POST["message"]);

	// Check that data was sent to the mailer.
	if ( empty($name) OR empty($message) OR !filter_var($email, FILTER_VALIDATE_EMAIL)) {
		// Set a 400 (bad request) response code and exit.
		http_response_code(400);
		echo "Oops! There was a problem with your submission. Please complete the form and try again.";
		exit;
	}
		
	require_once "PHPMailer.php";

	$mail = new PHPMailer;
	
	$recipient = $recipient_mail;
	$recipient_name = $recipient_name;

	// Set the email subject.
	$subject = "Induzy contact from $name";

	// Build the email content.
	$email_content = "Name: $name\n";
	$email_content .= "Email: $email\n";
	$email_content .= "Phone: $phone\n";
	$email_content .= "Subject: $subject\n";
	$email_content .= "Message:\n$message\n";
	
	
	$mail->From = $email;
	$mail->FromName = $name;
	$mail->addAddress($recipient, $recipient_name);
	
	//Provide file path and name of the attachments
	//$mail->addAttachment($user_file, $userfile_name);        
	$mail->isHTML(true);
	$mail->Subject = $subject;
	$mail->Body = nl2br( $email_content );
	//$mail->AltBody = "This is the plain text version of the email content";
	
	if(!$mail->send()){
		echo "Mailer Error: " . $mail->ErrorInfo;
	}else{
		echo "Message has been sent successfully";
	}
	exit;
	

} else {
	// Not a POST request, set a 403 (forbidden) response code.
	http_response_code(403);
	echo "There was a problem with your submission, please try again.";
	exit;
}


function rudr_mailchimp_curl_connect( $url, $apiKey, $json = '' ) {
		
	$headers = array(
		'Content-Type: application/json'
	);
	
	$ch = curl_init($url);
	curl_setopt($ch, CURLOPT_USERPWD, 'user:' . $apiKey);
	curl_setopt($ch, CURLOPT_HTTPHEADER, ['Content-Type: application/json']);
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
	curl_setopt($ch, CURLOPT_TIMEOUT, 10);
	curl_setopt($ch, CURLOPT_CUSTOMREQUEST, 'PUT');
	curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
	curl_setopt($ch, CURLOPT_POSTFIELDS, $json);
	$result = curl_exec($ch);
	$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
	curl_close($ch);
	
	return $httpCode;
	
}