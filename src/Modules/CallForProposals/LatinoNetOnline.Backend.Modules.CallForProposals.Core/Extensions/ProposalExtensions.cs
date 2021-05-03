using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Emails;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;

using System.Text;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions
{
    static class ProposalExtensions
    {
        public static ProposalDto ConvertToDto(this Proposal proposal)
            => new(proposal.Id, proposal.Title, proposal.Description, proposal.EventDate, proposal.CreationTime, proposal.AudienceAnswer, proposal.KnowledgeAnswer, proposal.UseCaseAnswer);

        public static ProposalFullDto ConvertToFullDto(this Proposal proposal)
            => new(proposal.ConvertToDto(), proposal.Speaker.ConvertToDto());

        public static SendEmailInput ConvertToEmailInput(this ProposalFullDto proposal)
        {
            StringBuilder message = new();
            message.AppendLine($"Speaker: {proposal.Speaker.Name} {proposal.Speaker.LastName}");
            message.AppendLine($"Charla: { proposal.Proposal.Title}");
            message.AppendLine($"Fecha: { proposal.Proposal.EventDate:dd/MM/yyyy}");

            string html = @$"<!DOCTYPE html>
<html>

<head>
	<meta charset='utf-8'>
	<meta name='viewport' content='width=device-width, initial-scale=1, maximum-scale=1'>
	<title>Call For Speakers | Latino .NET Online</title>
	<meta name='title' content='Call For Speakers | Latino .NET Online'>
	<meta name='description'
		content='Desde el equipo de organización estamos agradecidos de que dediques tu tiempo para proponer una charla para dar en uno de nuestros sábados de webinar.'>
	<link rel='shortcut icon' type='image/png' href='https://latinonet.online/favicon.png' />

    
	<link rel='stylesheet' type='text/css' href='https://latinonet.online/callforspeakers/css/opensans-font.css'>
	<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css'>

	<link rel='stylesheet' type='text/css'
		href='https://latinonet.online/callforspeakers/fonts/material-design-iconic-font/css/material-design-iconic-font.min.css'>
	<link rel='stylesheet' href='https://latinonet.online/callforspeakers/css/style.css' />
</head>

<body>
	<div class='page-content'>
		<div class='form-v1-content'>
			<div class='wizard-form'>
				<form class='form-register' action='#' method='post'>
					<div id='form-total'>
                        <div id='thankyou'>
							<div class='inner'>

								<div class='wizard-header'>
									<h1 id='thankyou-title' class='heading welcome' style='text-align: center;'>Muchas Gracias {proposal.Speaker.Name} Por Postular
										Su Charla</h1>

									<p style='margin-top: 50px; text-align: center;'>Pronto estaremos contactandonos con
										usted para devolverle feedback!
									</p>
									<p style='text-align: center;'>
										No olvide seguirnos en nuestras redes sociales 😉
									</p>

									<div style='text-align: center;'>
										<a href='https://www.facebook.com/LatinoNETOnline' target='_blank' class='fa fa-facebook'></a>
										<a href='https://twitter.com/latinonetonline' target='_blank' class='fa fa-twitter'></a>
									</div>

								</div>
							</div>
						</div>
						<section>
							<div class='inner'>
                                <div class='wizard-header'>
									<h4 class='heading' style='text-align: center;'>Sobre el Speaker</h4>
								</div>
								<div class='contenerdor-resumen'>
									<div class='form-row'>
										<div class='form-holder'>
											<img width='200px' src='{proposal.Speaker.Image}' id='confirmacion-imagen' />
										</div>
										<div class='form-holder'>
											<h4 class='word-break' id='confirmacion-nombre'>{proposal.Speaker.Name} {proposal.Speaker.LastName}</h4>
											<p class='word-break' id='confirmacion-email'>{proposal.Speaker.Email}</p>
											<p class='word-break' id='confirmacion-twitter'>{proposal.Speaker.Twitter}</p>
										</div>
									</div>
									<div class='form-row'>
										<div class='form-holder form-holder-2'>
											<p class='word-break' id='confirmacion-descripcion'>{proposal.Speaker.Description}</p>
										</div>
									</div>
								</div>
								<div class='wizard-header'>
									<h4 class='heading' style='text-align: center;'>Subre tu charla</h4>
								</div>
								<div class='contenerdor-resumen'>
									<div class='form-row'>
										<div class='form-holder form-holder-2'>
											<p class='word-break' id='confirmacion-fecha'>{proposal.Proposal.EventDate:dd/MM/yyyy}</p>
											<h4 class='word-break' id='confirmacion-titulo'>{proposal.Proposal.Title}</h4>
										</div>
									</div>
									<div class='form-row'>
										<div class='form-holder form-holder-2'>
											<p class='word-break' id='confirmacion-charla-descripcion'>{proposal.Proposal.Description}</p>
										</div>
									</div>
								</div>

								<div class='wizard-header'>
									<h4 class='heading' style='text-align: center;'>Más detalles</h4>
								</div>
								<div class='contenerdor-resumen'>
									<div class='form-row'>
										<div class='form-holder form-holder-2'>
											<p class='word-break'><b>¿Para quien es esta charla?</b></p>
											<p class='word-break' id='confirmacion-respuesta1'>{proposal.Proposal.AudienceAnswer}</p>

											<p class='word-break'><b>¿Que podre hacer con este nuevo conocimiento?</b></p>
											<p class='word-break' id='confirmacion-respuesta2'>{proposal.Proposal.KnowledgeAnswer}</p>

											<p class='word-break'><b>¿Te animas a contarnos un caso de uso?</b></p>
											<p class='word-break' id='confirmacion-respuesta3'>{proposal.Proposal.UseCaseAnswer}</p>
										</div>
									</div>
								</div>
							</div>
						</section>
					</div>
				</form>
			</div>
		</div>
	</div>

	<script src='https://latinonet.online/callforspeakers/js/jquery-3.3.1.min.js'></script>
	<script src='https://latinonet.online/callforspeakers/js/jquery.steps.js'></script>
</body>

</html>";

            return new SendEmailInput($"Confirmación del Call For Speaker de Latino .NET Online",
		  proposal.Speaker.Email,
          message.ToString(),
		  html);
        }
    }
}
