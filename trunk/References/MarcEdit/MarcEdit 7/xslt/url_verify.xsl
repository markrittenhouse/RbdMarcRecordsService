<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<html>
			<head>
				<title>URL Checker Results</title>
				<style>
					html {
					    font-size: 62.5%;
					    overflow-y: scroll;
					    -webkit-text-size-adjust: 100%;
					    -ms-text-size-adjust: 100%;
					}
					body {
					    background-color: #ccc;
					}
					a,
					a:link,
					a:active
					{
					    text-decoration: none;
					}
					a:hover {
					    text-decoration: underline;
					}
					
					table {
					    margin: auto;
					    width: 70%;
					    background-color: white;
					    border-collapse: collapse;
					    border: 1px solid black;
					}
					
					
					td {
					    text-align: center;
					    vector-effect: top;
					    word-wrap: normal;
					}
					table td {
					    border: 1px solid black;
					}
					
					div {
					    background-color: white;
					}</style>
				<script>
        
        
        function myFunction() {
        var x = document.getElementById("rawtext");
        if (x.style.display === "none") {
        x.style.display = "block";
        } else {
        x.style.display = "none";
        }
        }
        
      </script>
			</head>
			<body>
				<div style="width:80%;margin:auto;">
					<div style="text-align:right;">
						<img
							src="http://marcedit.reeset.net/wp-content/uploads/2013/03/header_marc4.png"
						/>
					</div>
					<div style="text-align:center;">
						<h1>MarcEdit URL Validation Results</h1>
					</div>
					<br/>
					<br/>
					<table>
						<tr>
							<td colspan="5">
								<b>Errors:</b>
							</td>
						</tr>
						<tr>
							<td>
								<b>Record #</b>
							</td>
							<td>
								<b>Display Field</b>
							</td>
							<td>
								<b>URL</b>
							</td>
							<td>
								<b>Status Code</b>
							</td>
							<td>
								<b>Status Message</b>
							</td>
						</tr>
						<xsl:call-template name="url_validate_errors"/>						
						<tr>
							<td colspan="5">
								<br/>
							</td>
						</tr>
						<tr>
							<td colspan="5">
								<b>Validated:</b>
							</td>
						</tr>
						<tr>
							<td>
								<b>Record #</b>
							</td>
							<td>
								<b>Display Field</b>
							</td>
							<td>
								<b>URL</b>
							</td>
							<td>
								<b>Status Code</b>
							</td>
							<td>
								<b>Status Message</b>
							</td>
						</tr>
						<xsl:call-template name="url_validate_nonerrors"/>
					
					<tr>
					
					
					
					<td style="text-align:left;" colspan="5">
						<a style="text-align:left;" href="#" onclick="myFunction();">Show Values</a>
					<div id="rawtext" style="text-align:left;display:none;">
						Error Record Numbers:
						<br />
						<textarea style="width:80%;height:150px;">
							<xsl:call-template name="url_validate_errors_text"/>
						</textarea>
						<br />
						<br />
						Validated Record Numbers:
						<br />
						<textarea style="width:80%;height:150px;">
							<xsl:call-template name="url_validate_nonerrors_text"/>
						</textarea>
					</div>
					</td>
					</tr>
					</table>
				</div>
			</body>
		</html>
	</xsl:template>

	<xsl:template name="url_validate_errors_text">
		<xsl:for-each select="url_validate">
			<xsl:for-each select="record">
				<xsl:sort select="record_number" case-order="upper-first" order="ascending"/>
				<xsl:sort select="statuscode"/>
				<xsl:if test="statuscode != '200'">
					<xsl:value-of select="record_number" /><xsl:text>&#xa;</xsl:text>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	
	
	<xsl:template name="url_validate_errors">
		<xsl:for-each select="url_validate">
			<xsl:for-each select="record">
				<xsl:sort select="record_number" case-order="upper-first" order="ascending"/>
				<xsl:sort select="statuscode"/>
				<xsl:if test="statuscode != '200'">
					<tr>
						<td>
							<xsl:value-of select="record_number"/>
						</td>
						<td>
							<xsl:value-of select="normalize-space(title/.)"/>
						</td>
						<td>
							<a>
								<xsl:attribute name="href">
									<xsl:value-of select="normalize-space(url/.)"/>
								</xsl:attribute>
								<xsl:value-of select="normalize-space(url/.)"/>
							</a>
						</td>
						<td>
							<xsl:value-of select="normalize-space(translate(statuscode/., ';', ''))"
							/>
						</td>
						<td><xsl:value-of
								select="normalize-space(translate(statusmessage/., ';', ''))"/>;
								<xsl:value-of select="normalize-space(errormessage/.)"/></td>
					</tr>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="url_validate_nonerrors">
		<xsl:for-each select="url_validate">
			<xsl:for-each select="record">
				<xsl:sort select="record_number" case-order="upper-first" order="ascending"/>
				<xsl:sort select="statuscode"/>
				<xsl:if test="statuscode = '200'">
					<tr>
						<td>
							<xsl:value-of select="record_number"/>
						</td>
						<td>
							<xsl:value-of select="normalize-space(title/.)"/>
						</td>
						<td>
							<a>
								<xsl:attribute name="href">
									<xsl:value-of select="normalize-space(url/.)"/>
								</xsl:attribute>
								<xsl:value-of select="normalize-space(url/.)"/>
							</a>
						</td>
						<td>
							<xsl:value-of select="normalize-space(translate(statuscode/., ';', ''))"
							/>
						</td>
						<td><xsl:value-of
								select="normalize-space(translate(statusmessage/., ';', ''))"/>;
								<xsl:value-of select="normalize-space(errormessage/.)"/></td>
					</tr>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="url_validate_nonerrors_text">
		<xsl:for-each select="url_validate">
			<xsl:for-each select="record">
				<xsl:sort select="record_number" case-order="upper-first" order="ascending"/>
				<xsl:sort select="statuscode"/>
				<xsl:if test="statuscode = '200'">
					<xsl:value-of select="record_number" /><xsl:text>&#xa;</xsl:text>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>
