function removeProduct(element) {
	var selectedRow = $(element).parent().closest('tr').remove();
}

function clearTextbox(textbox) {
	$(textbox).dblclick(function () {
		$(this).val('');
	});
}

function addProduct() {
	var selectedProduct = $('#selected_product').find(':selected');
	var selectedId = selectedProduct.val();
	var selectedText = selectedProduct.text();

	var newRow = '<tr><td id="product_' + selectedId + '"' + '>' + selectedText + '</td>'
		+ '<td><input type="text" class="amount form-control" ondblclick=clearTextbox(this) /></td>'
		+ '<td><input type="text" class="price form-control" ondblclick=clearTextbox(this) /></td>'
		+ '<td><a href="#" onclick="removeProduct(this)">'
		+ '<i class="fa fa-times fa-2x" aria-hidden="true"></i></a></td></tr>';
	var tbody = $('#products-table tbody').append(newRow);

	$('#check_btn').show();
}

function checkHandler() {
	if (customFieldsValidation()) {
		makePurchase();
	}
}

function getPurchaseJSON() {
	var allRows = $('#products-table tbody tr');
	var JSON = {};

	for (var i = 0; i < allRows.length; i++) {
		JSON["item" + i] = {};

		var productId = allRows[i].firstChild.id; //get tag id like "product_1"
		var numberId = productId.indexOf('_') + 1; //search index after _
		JSON["item" + i]["productId"] = productId.slice(numberId); // get actual productId

		JSON["item" + i]["amount"] = allRows[i].getElementsByClassName("amount")[0].value;
		JSON["item" + i]["price"] = allRows[i].getElementsByClassName("price")[0].value;
	}
	return JSON;
}

function customFieldsValidation() {
	var isValid = true;

	var numReg = /^\d+$/g;
	var amounts = $(".amount");
	for (var i = 0; i < amounts.length; i++) {
		if (!amounts[i].value.match(numReg)) {
			amounts[i].value = "Field must be an integer."
			isValid = false;
		}
	}

	var decReg = /^\d+(\.\d{1,2})?$/i;
	var prices = $(".price");
	for (var j = 0; j < prices.length; j++) {
		if (!prices[j].value.match(decReg)) {
			prices[j].value = "Field must be a decimal currency."
			isValid = false;
		}
	}
	return isValid;
}

function makePurchase() {
	var object = getPurchaseJSON();
	$.ajax({
		async: true,
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: ajaxUrl,
		data: JSON.stringify(object),
		success: function (data) {
			showCheckGui(data);
		},
		error: function (ajaxContext) {
			alert("Something is wrong.")
		}
	});
}

function showCheckGui(data) {
	displayCheck(data);
	$("#check_btn").hide();
	$('#new_purchase_btn').show();
	$('#selected_product').hide();
	$('#add_btn').hide();
}

function showPurchaseGui() {
	$('#selected_product').show();
	$('#check_btn').hide();
	$('#products-table tbody').html("");
	$('#add_btn').show();
	$('#new_purchase_btn').hide();
}

function displayCheck(data) {
	var tbody = $('#products-table tbody')
	tbody.html("");
	var dataObject = JSON.parse(data);
	var dateTime = dataObject["DateTime"];
	var productInfo = dataObject["Products"];
	var totalPrice = dataObject["TotalPrice"];
	for (var item in productInfo) {
		var newRow = '<tr><td>' + productInfo[item]["Caption"] + '</td>'
			+ '<td >' + productInfo[item]["Amount"] + '</td>'
			+ '<td>' + productInfo[item]["Price"] + '</td></tr>';
		tbody.append(newRow);
	}
	var priceRow = '<tr><td colspan = 3 align="right"> Total price: ' + totalPrice + '<td></tr>';
	var dateRow = '<tr><td colspan = 3 align="right"> Date time: ' + dateTime + '<td></tr>';
	tbody.append(priceRow);
	tbody.append(dateRow);
}